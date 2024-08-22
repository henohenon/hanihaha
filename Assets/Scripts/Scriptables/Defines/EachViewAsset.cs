using System;
using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using Alchemy.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteViewData", menuName = "Scriptable/View/SpriteView")]
public class SpriteViewAsset : EachViewAsset<Sprite> 
{
}

[AlchemySerialize]
public partial class EachViewAsset<T> : ScriptableObject
{
    [AlchemySerializeField, NonSerialized]
    public Dictionary<T, List<string>> _wardViewValues;
    
    [Button]
    public void Clear()
    {
        _wardViewValues.Clear();
    }
    
    public void AddEachWard(T each, string ward)
    {
        if (!_wardViewValues.ContainsKey(each))
        {
            _wardViewValues.Add(each, new ());
        }
        
        var values = _wardViewValues[each];
        
        values.Add(ward);
    }
    
    public void RemoveEachWard(T each, string ward)
    {
        if (!_wardViewValues.ContainsKey(each)) return;
        
        var values = _wardViewValues[each];
        values.Remove(ward);
    }
    
    
    public T GetRandom()
    { 
        var keys = _wardViewValues.Keys.ToList();
        Debug.Log(keys.Count);
        var randomIndex = UnityEngine.Random.Range(0, keys.Count-1);
        return keys[randomIndex];
    }

    public bool IsSame(T a, T b)
    {
        var aWards = _wardViewValues[a];
        var bWards = _wardViewValues[b];
        foreach (var aWard in aWards)
        {
            foreach (var bward in bWards)
            {
                if (aWard == bward)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool HasWord(T each, string ward)
    {
        var wards = _wardViewValues[each];
        return wards.Contains(ward);
    }
}
