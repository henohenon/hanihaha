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
    public void Clear()
    {
        wards.Clear();
    }
    
    public int GetOrMinus(string ward)
    {
        if(ward == "") return -1;
        return wards.IndexOf(ward);
    }
    
    public int GetOrAdd(string ward)
    {
        var index = wards.IndexOf(ward);
        if (index == -1)
        {
            wards.Add(ward);
            index = wards.Count - 1;
            Debug.Log("Add ward: " + ward);
        }
        return index;
    }
    
    public bool RemoveWard(string ward)
    {
        var index = wards.IndexOf(ward);
        
        if (index == -1) return false;

        wards[index] = "";
        
        Debug.Log("Remove ward: " + ward);
        return true;
    }
}
