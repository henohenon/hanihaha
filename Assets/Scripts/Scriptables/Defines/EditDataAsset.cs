using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Alchemy.Serialization;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;
using Unity.VisualScripting;


[AlchemySerialize]
[CreateAssetMenu(fileName = "EditData", menuName = "Scriptable/EditDataAsset")]
public partial class EditDataAsset : ScriptableObject
{
    [BoxGroup("Functions")] public string wardInput = "";
    [BoxGroup("Functions")] public AnswerCardController prefabInput;


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
        var wardIndex = wardData.wards.IndexOf(wardInput);
        var wardAdded = false;

        // ワードが存在しない場合、追加
        if (wardIndex == -1)
        {
            wardData.wards.Add(wardInput);
            wardIndex = wardData.wards.Count - 1;
            wardAdded = true;
            Debug.Log("Add wardInput: " + wardInput);
        }

        // プレハブの追加
        var prefabChanged = AddEachValue(wardIndex, prefabInput, prefabData);

        // どちらも追加されていない場合、警告
        if (!wardAdded && !prefabChanged)
        {
            Debug.LogWarning("Already added exists wardInput: " + wardInput + " prefabInput: " + prefabInput);
        }
    }

    // 各種データの追加関数
    private bool AddEachValue<T>(int wardIndex, T eachInput, EachDataAsset<T> eachdData)
    {
        // 入力がnullの場合、false
        if (eachInput == null) return false;

        // 入力の位置取得
        var eachIndex = eachdData.list.IndexOf(eachInput);
        // 入力が存在しない場合、追加
        if (eachIndex == -1)
        {
            eachdData.list.Add(eachInput);
            eachIndex = eachdData.list.Count - 1;
            Debug.Log("Add eachInput: " + eachInput);
        }


        // ペアの追加
        var exitsPairs = eachdData.ward_link.Where(pair => pair.wardId == wardIndex && pair.valueId == eachIndex)
            .ToList();

        if (exitsPairs.Count == 0)
        {
            eachdData.ward_link.Add(new WardValuePair(wardIndex, eachIndex));
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
        var wardIndex = wardData.wards.IndexOf(wardInput);
        if (wardIndex == -1)
        {
            Debug.LogError("Please set exist ward!");
            return;
        }

        // プレファブがなければ
        if (prefabInput == null)
        {
            // ワード_プレファブペアの削除
            RemoveWardPairList(ref prefabData, wardIndex);
            // ワードの削除
            wardData.wards.RemoveAt(wardIndex);
            Debug.Log("Remove ward: " + wardInput);
            return;
        }
        else
        {
            var prefabRemoved = RemoveInputPair<AnswerCardController>(wardIndex, prefabInput, ref prefabData);

            if (!prefabRemoved)
            {
                Debug.LogWarning("Not exists wardInput: " + wardInput + " prefabInput: " + prefabInput);
            }
        }
    }

    private bool RemoveInputPair<T>(int wardIndex, T eachInput, ref EachDataAsset<T> eachData)
    {
        var eachIndex = eachData.list.IndexOf(eachInput);
        if (eachIndex == -1) return false;

        // 削除する一覧
        var removePairs = eachData.ward_link.Where(pair => pair.wardId == wardIndex && pair.valueId == eachIndex)
            .ToList();
        foreach (var pair in removePairs)
        {
            Debug.Log("Remove ward_each: " + wardInput + " - " + eachInput);
        }

        // 削除
        eachData.ward_link = eachData.ward_link.Where(pair => !(pair.wardId == wardIndex && pair.valueId == eachIndex))
            .ToList();

        return removePairs.Count > 0;
    }

    private void RemoveWardPairList<T>(ref EachDataAsset<T> data, int wardIndex)
    {
        // 削除する一覧
        var removePairs = data.ward_link.Where(pair => pair.wardId == wardIndex).ToList();
        foreach (var pair in removePairs)
        {
            Debug.Log("Remove ward_each: " + wardInput + " - " + pair.valueId);
        }

        if (removePairs.Count == 0) return;
        // 削除
        data.ward_link = data.ward_link.Where(pair => pair.wardId != wardIndex).ToList();
    }


    [BoxGroup("Views")] [InlineEditor] public WardViewAsset wardView;

    [BoxGroup("Views")]
    [Button]
    private void ReGenerate()
    {

    }


    [BoxGroup("Datas")] [InlineEditor] public WardDataAsset wardData;
    [BoxGroup("Datas")] [InlineEditor] public EachDataAsset<AnswerCardController> prefabData;

    [BoxGroup("Datas")]
    [FoldoutGroup("Datas/Clear")]
    [Button]
    private void Clear()
    {
        wardData.wards.Clear();
        prefabData.list.Clear();
        prefabData.ward_link.Clear();
    }
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