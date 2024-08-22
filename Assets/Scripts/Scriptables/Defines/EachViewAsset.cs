using System;
using System.Collections.Generic;
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
}
