using UnityEngine;
using System.Collections.Generic;
using R3;

public class AnswerCardsManager
{
    private List<AnswerCardController> _answerCards;

    public void RecreateCards()
    {
        
    }

    public void CreateAnswerCard(AnswerCardController prefab, bool isSame)
    {
        AnswerCardController card = Object.Instantiate(prefab);
        _answerCards.Add(card);
        
        card.onClick.Subscribe(_ =>
        {
            if (isSame)
            {
                Debug.Log("Correct!");
            }
            else
            {
                Debug.Log("Incorrect!");
            }
        }).AddTo(card);
    }
}
