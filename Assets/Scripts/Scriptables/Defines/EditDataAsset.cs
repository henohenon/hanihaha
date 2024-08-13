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

        //  ワードの追加
        var wardIndex = wardData.GetOrAdd(wardInput);
        
        // プレハブの追加
        prefabData.AddEachValue(wardIndex, prefabInput);
    }



    [BoxGroup("Functions")]
    [HorizontalGroup("Functions/Buttons")]
    [Button]
    public void Remove()
    {
        // 存在しないワードならエラー
        var wardIndex = wardData.GetOrMinus(wardInput);
        if (wardIndex == -1)
        {
            Debug.LogError("Please set exist ward!");
            return;
        }

        // プレファブがなければ
        if (prefabInput == null)
        {
            // プレファブのペアの削除
            prefabData.RemoveWardPairList(wardIndex);
            // ワードの削除
            wardData.RemoveWard(wardInput);
            return;
        }
        // ワード_プレファブペアの削除
        var prefabRemoved = prefabData.RemoveInputPair(wardIndex, prefabInput);
        

        // 削除できなかったら警告
        if (!prefabRemoved)
        {
            Debug.LogWarning("Not exists wardInput: " + wardInput + " prefabInput: " + prefabInput);
            return;
        }
        var isPrefabHasWard = prefabData.HasWard(wardIndex);
        if (!isPrefabHasWard)
        {
            wardData.RemoveWard(wardInput);
        }
        
    }


    [BoxGroup("Views")] 
    [InlineEditor] public WardViewAsset wardView;
    
    [BoxGroup("Views")] 
    [Button]
    private void GenerateFromData()
    {
        wardView.Clear();
        foreach (var link in prefabData.ward_link)
        {
            if(link.wardId < 0 || link.wardId >= wardData.wards.Count || link.valueId < 0 || link.valueId >= prefabData.list.Count ) continue;
            wardView.AddWardPrefab(wardData.wards[link.wardId], prefabData.list[link.valueId]);
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
public class WardValueLink
{
    public int wardId;
    public int valueId;
    public WardValueLink(int wardId, int valueId)
    {
        this.wardId = wardId;
        this.valueId = valueId;
    }
    public WardValueLink()
    {
        this.wardId = wardId;
        this.valueId = valueId;
    }
}