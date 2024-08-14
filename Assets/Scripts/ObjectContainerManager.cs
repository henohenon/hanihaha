using UnityEngine;
using System.Collections.Generic;
using R3;

public class ObjectContainerManager
{
    private List<AnswerCardController> _answerCards = new ();

    public void CreateAnswerCard(AnswerCardProp prop, bool isSame)
    {
        var card = Object.Instantiate(prop.prefab);
        _answerCards.Add(card);
        
        card.onClick.Subscribe(_ =>
        {
            card.onAnswer.OnNext(isSame);
        }).AddTo(card);
    }
}

public class AnswerCardProp
{
    public AnswerCardController prefab;

    public AnswerCardProp(AnswerCardController prefab)
    {
        this.prefab = prefab;
    }
}