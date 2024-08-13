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
        var prefabChanged = prefabData.AddEachValue(wardIndex, prefabInput);

        // どちらも追加されていない場合、警告
        if (!wardAdded && !prefabChanged)
        {
            Debug.LogWarning("Already added exists wardInput: " + wardInput + " prefabInput: " + prefabInput);
        }
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
            prefabData.RemoveWardPairList(wardIndex);
            // ワードの削除
            wardData.wards.RemoveAt(wardIndex);
            Debug.Log("Remove ward: " + wardInput);
            return;
        }
        else
        {
            // プレファブのペアの削除
            var prefabRemoved = prefabData.RemoveInputPair(wardIndex, prefabInput);

            // 削除できなかったら警告
            if (!prefabRemoved)
            {
                Debug.LogWarning("Not exists wardInput: " + wardInput + " prefabInput: " + prefabInput);
            }
        }
    }


    [BoxGroup("Views")] 
    [InlineEditor] public WardViewAsset wardView;
    
    [BoxGroup("Views")] 
    [Button]
    private void ReGenerate()
    {
        wardView.Clear();
        foreach (var prefab in prefabData.ward_link)
        {
            wardView.AddWardPrefab(wardData.wards[prefab.wardId], prefabData.list[prefab.valueId]);
        }
    }

    [BoxGroup("Datas")]
    public bool isEdit = false;

    [BoxGroup("Datas")] [InlineEditor] [EnableIf("isEdit")] 
    public WardDataAsset wardData;
    [BoxGroup("Datas")] [InlineEditor] [EnableIf("isEdit")] 
    public EachDataAsset<AnswerCardController> prefabData;
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