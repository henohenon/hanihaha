using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Alchemy.Serialization;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;


[AlchemySerialize]
[CreateAssetMenu(fileName = "EditData", menuName = "Scriptable/EditDataAsset")]
public partial class EditDataAsset : ScriptableObject
{
    [BoxGroup("Functions")]
    public string wardInput = "";
    [BoxGroup("Functions")]
    public AnswerCardController prefabInput;


    [BoxGroup("Functions")]
    [HorizontalGroup("Functions/Buttons")]
    [Button]
    public void Add()
    {
        // 空白ならエラー
        if (wardInput == "")
        {
            Debug.LogError("Please set wardInput!");
            return;
        }
        
        //  ワードの位置取得
        var wardIndex = answerData.wards.IndexOf(wardInput);
        var wardAdded = false;
        
        // ワードが存在しない場合、追加
        if (wardIndex == -1)
        {
            answerData.wards.Add(wardInput);
            wardIndex = answerData.wards.Count - 1;
            wardAdded = true;
            Debug.Log("Add wardInput: " + wardInput);
        }
        
        // プレハブの追加
        var prefabChanged = AddEachValue(wardIndex, prefabInput, answerData.prefabs, answerData.ward_prefabs);
        
        // どちらも追加されていない場合、警告
        if (!wardAdded && !prefabChanged)
        {
            Debug.LogWarning("Already added exists wardInput: " + wardInput + " prefabInput: " + prefabInput);
        }
    }

    // 各種データの追加関数
    private bool AddEachValue<T>(int wardIndex, T eachInput, List<T> eachList, List<WardValuePair> ward_each)
    {
        // 入力がnullの場合、false
        if (eachInput == null) return false;
        
        // 入力の位置取得
        var eachIndex = eachList.IndexOf(eachInput);
        // 入力が存在しない場合、追加
        if (eachIndex == -1)
        {
            eachList.Add(eachInput);
            eachIndex = eachList.Count - 1;
            Debug.Log("Add eachInput: " + eachInput);
        }

        
        // ペアの追加
        var exitsPairs = ward_each.Where(pair => pair.wardId == wardIndex && pair.valueId == eachIndex).ToList();

        if( exitsPairs.Count == 0)
        {
            ward_each.Add(new WardValuePair(wardIndex, eachIndex));
            Debug.Log("Add ward_each: " + wardInput + " - " + eachInput);
            return true;
        }

        return false;
    }


    [BoxGroup("Functions")]
    [HorizontalGroup("Functions/Buttons")]
    [Button]
    public void Remove()
    {
        // 存在しないワードならエラー
        var wardIndex = answerData.wards.IndexOf(wardInput);
        if (wardIndex == -1)
        {
            Debug.LogError("Please set exist ward!");
            return;
        }

        // プレファブがなければ
        if (prefabInput == null)
        {
            // ワード_プレファブペアの削除
            RemoveWardPairList(answerData.ward_prefabs, wardIndex);
            // ワードの削除
            answerData.wards.RemoveAt(wardIndex);
            Debug.Log("Remove ward: " + wardInput);
            return;
        }
        else
        {
            var prefabRemoved = RemoveInputPair(answerData.prefabs, ref answerData.ward_prefabs, wardIndex, prefabInput);

            if (!prefabRemoved)
            {
                Debug.LogWarning("Not exists wardInput: " + wardInput + " prefabInput: " + prefabInput);
            }
        }
    }

    private bool RemoveInputPair<T>(List<T>eachList, ref List<WardValuePair> ward_each, int wardIndex, T eachInput)
    {
        var eachIndex = eachList.IndexOf(eachInput);
        if (eachIndex == -1) return false;

        // 削除する一覧
        var removePairs = ward_each.Where(pair => pair.wardId == wardIndex && pair.valueId == eachIndex).ToList();
        foreach (var pair in removePairs)
        {
            Debug.Log("Remove ward_each: " + wardInput + " - " + eachInput);
        }
        // 削除
        ward_each = ward_each.Where(pair => !(pair.wardId == wardIndex && pair.valueId == eachIndex)).ToList();
        
        return removePairs.Count > 0;
    }
    
    private void RemoveWardPairList(List<WardValuePair> pairs, int wardIndex)
    {
        // 削除する一覧
        var removePairs = pairs.Where(pair => pair.wardId == wardIndex).ToList();
        foreach (var pair in removePairs)
        {
            Debug.Log("Remove ward_each: " + wardInput + " - " + pair.valueId);
        }

        if (removePairs.Count == 0) return;
        // 削除
        pairs = pairs.Where(pair => pair.wardId != wardIndex).ToList();
    }
    
    
    [BoxGroup("Views")]
    [InlineEditor]
    public WardViewAsset wardView;

    [BoxGroup("Views")]
    [Button]
    private void ReGenerate()
    {
        
    }
    
    
    [BoxGroup("Datas")]
    [InlineEditor]
    public AnswerDataAsset answerData;
}


[Serializable]
public class WardValuePair
{
    public int wardId;
    public int valueId;
    public WardValuePair(int wardId, int valueId)
    {
        this.wardId = wardId;
        this.valueId = valueId;
    }
    public WardValuePair()
    {
        this.wardId = wardId;
        this.valueId = valueId;
    }
}