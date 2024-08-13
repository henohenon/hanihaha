using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Alchemy.Serialization;
using UnityEngine;

[AlchemySerialize]
[CreateAssetMenu(fileName = "AnswerData", menuName = "Scriptable/Data/WardData")]
public partial class WardDataAsset : ScriptableObject
{
    [ReadOnly]
    public List<string> wards = new ();
}

[CreateAssetMenu(fileName = "PrefabData", menuName = "Scriptable/Data/Each/PrefabData")]
public class PrefabDataAsset: EachDataAsset<AnswerCardController> { }

[AlchemySerialize]
public partial class EachDataAsset<T> : ScriptableObject
{
    [ReadOnly]
    public List<T> list = new ();
    
    [AlchemySerializeField, NonSerialized]
    [ReadOnly]
    public List<WardValuePair> ward_link = new ();
}