using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Alchemy.Serialization;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "EditData", menuName = "Scriptable/EditDataAsset")]
public partial class EditDataAsset : ScriptableObject
{
    [BoxGroup("Functions")] public string wardInput = "";
    [BoxGroup("Functions")] public Sprite spriteInput;


    [BoxGroup("Functions")]
    [HorizontalGroup("Functions/Buttons")]
    [Button]
    public void Add()
    {
        Debug.Log(wardInput);
        // 空白ならエラー
        if (wardInput == "")
        {
            Debug.LogError("Please set wardInput!");
            return;
        }

        //  ワードの追加
        var wardIndex = wardData.GetOrAdd(wardInput);
        
        // ペアの追加
        if (spriteData.AddInputPair(wardIndex, spriteInput))
        {
            // ビューに反映
            wardView.AddWardEach(wardInput, spriteInput);
            spriteView.AddEachWard(spriteInput, wardInput);
        }
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
        if (spriteInput == null)
        {
            // プレファブのペアの削除
            spriteData.RemoveWardPairList(wardIndex);
            // ワードの削除
            wardData.RemoveWard(wardInput);
            return;
        }
        // ワード_プレファブペアの削除
        var spriteRemoved = spriteData.RemoveInputPair(wardIndex, spriteInput);

        if (spriteRemoved)
        {
            wardView.RemoveWardEach(wardInput, spriteInput);
            spriteView.RemoveEachWard(spriteInput, wardInput);
        }

        // 削除できなかったら警告
        if (!spriteRemoved)
        {
            Debug.LogWarning("Not exists wardInput: " + wardInput + " spriteInput: " + spriteInput);
            return;
        }
        var isPrefabHasWard = spriteData.HasWard(wardIndex);
        if (!isPrefabHasWard)
        {
            wardData.RemoveWard(wardInput);
        }
    }
    
    [BoxGroup("Functions")]
    [Button]
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Save()
    {
#if UNITY_EDITOR

        EditorUtility.SetDirty(wardData);
        EditorUtility.SetDirty(spriteData);
        EditorUtility.SetDirty(wardView);
        EditorUtility.SetDirty(spriteView);
        AssetDatabase.SaveAssets();
#endif
    }


    [BoxGroup("Views")]
    public bool viewEdit = false;
    [BoxGroup("Views")] [EnableIf("viewEdit")]
    [InlineEditor] public WardViewAsset wardView;
    
    [BoxGroup("Views")] [EnableIf("viewEdit")]
    [InlineEditor] public SpriteViewAsset spriteView;

    
    [BoxGroup("Views")] 
    [Button]
    private void GenerateFromData()
    {
        wardView.Clear();
        foreach (var link in spriteData.ward_link)
        {
            if(link.wardId < 0 || link.wardId >= wardData.wards.Count || link.valueId < 0 || link.valueId >= spriteData.list.Count ) continue;
            wardView.AddWardEach(wardData.wards[link.wardId], spriteData.list[link.valueId]);
        }

        spriteView.Clear();
        foreach (var link in spriteData.ward_link)
        {
            if(link.wardId < 0 || link.wardId >= wardData.wards.Count || link.valueId < 0 || link.valueId >= spriteData.list.Count ) continue;
            spriteView.AddEachWard(spriteData.list[link.valueId], wardData.wards[link.wardId]);
        }
    }

    [BoxGroup("Datas")]
    public bool dataEdit = false;

    [BoxGroup("Datas")] [InlineEditor] [EnableIf("dataEdit")]
    public WardDataAsset wardData;
    [BoxGroup("Datas")] [InlineEditor] [EnableIf("dataEdit")]
    
    public PrefabDataAsset spriteData;

    [BoxGroup("Datas")]
    [Button]
    private void GenerateFromView()
    {
        wardData.Clear();
        foreach (var ward in wardView._wardViewValues.Keys)
        {
            wardData.GetOrAdd(ward);
        }

        spriteData.Clear();
        foreach (var pair in spriteView._wardViewValues)
        {
            spriteData.AddListItem(pair.Key);

            foreach (var ward in pair.Value)
            {
                var wardIndex = wardData.GetOrMinus(ward);
                if (wardIndex == -1) continue;
                spriteData.AddInputPair(wardIndex, pair.Key);
            }
        }

    }
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