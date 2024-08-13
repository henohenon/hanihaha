using R3;
using UnityEngine;

public class TargetManager
{
    private readonly AnswerDataAsset _answerDataAsset;
    
    public TargetManager(AnswerDataAsset answerDataAsset)
    {
        _answerDataAsset = answerDataAsset;
    }
    
    public void UpdateTarget()
    {
        var randomIndex = Random.Range(0, _answerDataAsset.answerWords.Length);
        var answerWordData = _answerDataAsset.answerWords[randomIndex];
        Debug.Log(answerWordData.word);
    }
}
