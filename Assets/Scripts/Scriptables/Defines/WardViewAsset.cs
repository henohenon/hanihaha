using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Alchemy.Serialization;
using UnityEngine;

[AlchemySerialize]
[CreateAssetMenu(fileName = "WardViewData", menuName = "Scriptable/View/WardView")]
public partial class WardViewAsset : ScriptableObject
{
    [AlchemySerializeField, NonSerialized]
    Dictionary<string, WardViewValues> _wardViewValues = new ();
    
    [Button]
    public void Clear()
    {
        _wardViewValues.Clear();
    }
    
    public void AddWardPrefab(string ward, AnswerCardController prefab)
    {
        if (!_wardViewValues.ContainsKey(ward))
        {
            _wardViewValues.Add(ward, new WardViewValues());
        }
        
        var values = _wardViewValues[ward];
        
        values.wardPrefabs.Add(prefab);
    }
    
    public void RemoveWardPrefab(string ward, AnswerCardController prefab)
    {
        if (!_wardViewValues.ContainsKey(ward)) return;
        
        var values = _wardViewValues[ward];
        values.wardPrefabs.Remove(prefab);
    }
}

public class WardViewValues
{
    public List<AnswerCardController> wardPrefabs = new ();
}