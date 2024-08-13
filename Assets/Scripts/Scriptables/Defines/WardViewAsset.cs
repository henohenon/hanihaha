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
        var values = _wardViewValues[ward];
        if (values == null)
        {
            values = new WardViewValues();
            _wardViewValues.Add(ward, values);
        }
        values.wardPrefabs.Add(prefab);
    }
    
    public void RemoveWardPrefab(string ward, AnswerCardController prefab)
    {
        var values = _wardViewValues[ward];
        if (values == null) return;
        values.wardPrefabs.Remove(prefab);
    }
}

public class WardViewValues
{
    public List<AnswerCardController> wardPrefabs = new ();
}