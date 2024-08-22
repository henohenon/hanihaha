
using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Alchemy.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteData", menuName = "Scriptable/Data/Each/SpriteData")]
public class PrefabDataAsset: EachDataAsset<Sprite> { }

public partial class EachDataAsset<T> : ScriptableObject
{
    public List<T> list = new ();
    
    public List<WardValueLink> ward_link = new ();
    
    [Button]
    public void Clear()
    {
        list.Clear();
        ward_link.Clear();
    }
    
    public bool AddListItem(T item)
    {
        if (list.Contains(item)) return false;
        list.Add(item);
        
        Debug.Log("Add eachInput: " + item);
        
        return true;
    }
    
    public bool RemoveListItem(T item)
    {
        var index = list.IndexOf(item);
        
        if (index == -1) return false;

        list[index] = default(T);
        
        Debug.Log("Remove eachInput: " + item);
        
        return true;
    }
    
    // ペアの追加
    public bool AddInputPair(int wardIndex, T eachInput)
    {
        // 入力がnullの場合、false
        if (eachInput == null) return false;


        var isAdded = AddListItem(eachInput);
        // 入力の位置取得
        var eachIndex = list.IndexOf(eachInput);
        // 入力が存在しない場合、追加
        if (eachIndex == -1)
        {
            eachIndex = list.Count - 1;
        }
        
        // すでに存在する場合、false
        foreach (var link in ward_link)
        {
            if (link.wardId == wardIndex && link.valueId == eachIndex)
            {
                return false;
            }
        }
        
        // リンクの追加
        ward_link.Add(new WardValueLink(wardIndex, eachIndex));
        Debug.Log("Add ward_each: " + wardIndex + " - " + eachIndex);
        return true;
    }
    
    // 特定のペア削除
    public bool RemoveInputPair(int wardIndex, T eachInput)
    {
        var eachIndex = list.IndexOf(eachInput);
        if (eachIndex == -1) return false;

        var isRemoved = false;
        var eachCounts = 0;
        // 削除する一覧
        foreach (var link in ward_link)
        {
            if (link.wardId == wardIndex && link.valueId == eachIndex)
            {
                link.valueId = -1;
                link.wardId = -1;
                Debug.Log("Remove ward_each: " + wardIndex + " - " + eachInput);
                isRemoved = true;
            }else if(link.valueId == eachIndex)
            {
                eachCounts++;
            }
        }
        
        if(eachCounts == 0)
        {
            RemoveListItem(eachInput);
        }

        return isRemoved;
    }
    
    // 特定のワードのペア全削除
    public void RemoveWardPairList(int wardIndex)
    {
        foreach (var link in ward_link)
        {
            if (link.wardId == wardIndex)
            {
                link.wardId = -1;
            }
        }
    }

    public bool HasWard(int wardIndex)
    {
        foreach (var link in ward_link)
        {
            if (link.wardId == wardIndex)
            {
                return true;
            }
        }
        
        return false;
    }
}