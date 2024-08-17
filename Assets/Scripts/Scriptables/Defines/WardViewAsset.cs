using System;
using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using Alchemy.Serialization;
using UnityEditorInternal;
using UnityEngine;

[AlchemySerialize]
[CreateAssetMenu(fileName = "WardViewData", menuName = "Scriptable/View/WardView")]
public partial class WardViewAsset : ScriptableObject
{
    [AlchemySerializeField, NonSerialized]
    public Dictionary<string, WardViewValues> _wardViewValues = new ();
    
    [Button]
    public void Clear()
    {
        _wardViewValues.Clear();
    }
    
    public void AddWardEach<T>(string ward, T value)
    {
        if (!_wardViewValues.ContainsKey(ward))
        {
            _wardViewValues.Add(ward, new WardViewValues());
        }
        
        var values = _wardViewValues[ward];
        values.AddEach(value);
    }
    
    public void RemoveWardEach<T>(string ward, T value)
    {
        if (!_wardViewValues.ContainsKey(ward)) return;
        
        var values = _wardViewValues[ward];
        values.RemoveEach(value);
    }
    
    public string GetRandomWard()
    { 
        var keys = _wardViewValues.Keys.ToList();
        var randomIndex = UnityEngine.Random.Range(0, keys.Count);
        return keys[randomIndex];
    }

    public AnswerCardProp GetCorrectAnswerProp(string ward)
    {
        var values = _wardViewValues[ward];
        var prefabs = values.wardSprites;
        var randomIndex = UnityEngine.Random.Range(0, prefabs.Count);
        return new AnswerCardProp(prefabs[randomIndex]);
    }
}

public class AnswerCardProp
{
    public Sprite sprite;

    public AnswerCardProp(Sprite sprite)
    {
        this.sprite = sprite;
    }
}

public class WardViewValues
{
    public List<Sprite> wardSprites = new();
    
    public void AddEach<T>(T each)
    {
        switch (each)
        {
            case Sprite sprite:
                wardSprites.Add(sprite);
                break;
            default:
                Debug.LogError("AddEach type not found");
                break;
        }
    }
    
    public void RemoveEach<T>(T each)
    {
        switch (each)
        {
            case Sprite sprite:
                wardSprites.Remove(sprite);
                break;
            default:
                Debug.LogError("RemoveEach type not found");
                break;
        }
    }
}