using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnswerCardAsset", menuName = "AnswerCardAsset")]
public class AnswerDataAsset : ScriptableObject
{
    public AnswerWordData[] answerWords;
}


[System.Serializable]
public class AnswerWordData
{
    public string word;
    public GameObject[] answerPrefabs;
}