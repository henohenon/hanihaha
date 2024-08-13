using System;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    [SerializeField]
    private WardViewAsset _wardViewAsset;

    private AnswerCardsManager _answerCardsManager;
    
    private string targetWard;
    
    private void Start()
    {
        _answerCardsManager = new AnswerCardsManager();
        targetWard = _wardViewAsset.GetRandomWord();
        
        _answerCardsManager.CreateAnswerCard(new AnswerCardProp(_wardViewAsset._wardViewValues[targetWard].wardPrefabs[0]), true);
    }
}
