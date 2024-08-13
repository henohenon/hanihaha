using UnityEngine;
using System.Collections.Generic;
using R3;

public class AnswerCardsManager
{
    private List<AnswerCardController> _answerCards;

    public void RecreateCards()
    {
        
    }

    public void CreateAnswerCard(AnswerCardProp prop, bool isSame)
    {
        var card = Object.Instantiate(prop.prefab);
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
