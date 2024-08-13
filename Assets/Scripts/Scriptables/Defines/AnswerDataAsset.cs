using System;
using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using Alchemy.Serialization;
using UnityEngine;

[AlchemySerialize]
[CreateAssetMenu(fileName = "AnswerData", menuName = "Scriptable/Data/WardData")]
public partial class WardDataAsset : ScriptableObject
{
    public List<string> wards = new ();

    [Button]
    private void Clear()
    {
        wards.Clear();
    }
}

[CreateAssetMenu(fileName = "PrefabData", menuName = "Scriptable/Data/Each/PrefabData")]
public class PrefabDataAsset: EachDataAsset<AnswerCardController> { }

[AlchemySerialize]
public partial class EachDataAsset<T> : ScriptableObject
{
    public List<T> list = new ();
    
    [AlchemySerializeField, NonSerialized]
    public List<WardValuePair> ward_link = new ();
    
    [Button]
    private void Clear()
    {
        list.Clear();
        ward_link.Clear();
    }
    
    // ペアの追加
    public bool AddEachValue(int wardIndex, T eachInput)
    {
        // 入力がnullの場合、false
        if (eachInput == null) return false;

        // 入力の位置取得
        var eachIndex = list.IndexOf(eachInput);
        // 入力が存在しない場合、追加
        if (eachIndex == -1)
        {
            list.Add(eachInput);
            eachIndex = list.Count - 1;
            Debug.Log("Add eachInput: " + eachInput);
        }


        // ペアの追加
        var exitsPairs = ward_link.Where(pair => pair.wardId == wardIndex && pair.valueId == eachIndex)
            .ToList();

        if (exitsPairs.Count == 0)
        {
            ward_link.Add(new WardValuePair(wardIndex, eachIndex));
            Debug.Log("Add ward_each: " + wardIndex + " - " + eachIndex);
            return true;
        }

        return false;
    }
    
    // 特定のペア削除
    public bool RemoveInputPair(int wardIndex, T eachInput)
    {
        var eachIndex = list.IndexOf(eachInput);
        if (eachIndex == -1) return false;

        // 削除する一覧
        var removePairs = ward_link.Where(pair => pair.wardId == wardIndex && pair.valueId == eachIndex)
            .ToList();
        foreach (var pair in removePairs)
        {
            Debug.Log("Remove ward_each: " + wardIndex + " - " + eachInput);
        }

        // 削除
        ward_link = ward_link.Where(pair => !(pair.wardId == wardIndex && pair.valueId == eachIndex))
            .ToList();

        return removePairs.Count > 0;
    }
    
    // 特定のワードのペア全削除
    public void RemoveWardPairList(int wardIndex)
    {
        // 削除する一覧
        var removePairs = ward_link.Where(pair => pair.wardId == wardIndex).ToList();
        foreach (var pair in removePairs)
        {
            Debug.Log("Remove ward_each: " + wardIndex + " - " + pair.valueId);
        }

        if (removePairs.Count == 0) return;
        // 削除
        ward_link = ward_link.Where(pair => pair.wardId != wardIndex).ToList();
    }


}