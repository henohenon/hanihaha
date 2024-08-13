using UnityEngine;

[CreateAssetMenu(fileName = "AnswerCardAsset", menuName = "AnswerCardAsset")]
public class AnswerCardAsset : ScriptableObject
{
    [SerializeField]
    private string name;
    public string[] answeres;
    public GameObject prefab;
}
