using System.Collections.Generic;
using Alchemy.Inspector;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable/LevelData")]
public class LevelDataAsset : ScriptableObject
{
    [Header("BaseLevelProp")]
    public float eachPlusTime;
    public float eachMinusTime;
    public int maxCardNum;
    public float minCardNum;

    [TabGroup("Tab", "ImpactCard")]
    public bool isSpawnImpact;
    public AnimationCurve impactValueRate;
    public float impactMacValue;
    [TabGroup("Tab", "RotateCard")]
    public bool isSpawnRotate;
    public AnimationCurve rotateRate;
    [TabGroup("Tab", "ReflectionCard")]
    public bool isSpawnReflection;
    public AnimationCurve reflectionValueRate;
    public float reflectionMaxValue;
}