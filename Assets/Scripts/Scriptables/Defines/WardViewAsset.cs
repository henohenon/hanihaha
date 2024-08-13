using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Alchemy.Serialization;
using UnityEngine;

[AlchemySerialize]
[CreateAssetMenu(fileName = "WardViewData", menuName = "Scriptable/View/WardView")]
public partial class WardViewAsset : ScriptableObject
{
    [ReadOnly]
    [AlchemySerializeField, NonSerialized]
    Dictionary<string, WardViewValues> _wardViewValues = new ();
}

public class WardViewValues
{
    public GameObject[] wardPrefab;
}