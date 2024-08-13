using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Alchemy.Serialization;
using UnityEngine;

[AlchemySerialize]
[CreateAssetMenu(fileName = "AnswerData", menuName = "Scriptable/AnswerData")]
public partial class AnswerDataAsset : ScriptableObject
{
    [ReadOnly]
    public List<string> wards = new ();
    [ReadOnly]
    public List<AnswerCardController> prefabs = new ();
    
    [AlchemySerializeField, NonSerialized]
    [ReadOnly]
    public List<WardValuePair> ward_prefabs = new ();

    [Button]
    public void Clear()
    {
        wards.Clear();
        prefabs.Clear();
        ward_prefabs.Clear();
    }
}
