using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TargetsUIManager
{
    private List<AnswerCard> _alphaTargetCards;
    private List<AnswerCard> _betaTargetCards;
    private List<AnswerCard> _gammaTargetCards;
    private VisualElement _answerCardContainer;
    private VisualElement _footer;
    private VisualElement _game;
    
    public TargetsUIManager(VisualElement _root)
    {
        _alphaTargetCards = _root.Query<AnswerCard>().Class("TargetAlpha").ToList();
        _betaTargetCards = _root.Query<AnswerCard>().Class("TargetBeta").ToList();
        _gammaTargetCards = _root.Query<AnswerCard>().Class("TargetGamma").ToList();
        _answerCardContainer = _root.Q<VisualElement>("AnswerCards");
        _footer = _root.Q<VisualElement>("Footer");
        _game = _root.Q<VisualElement>("Game");
    }
    
    public void UpdateTargets(Sprite alphaSprite, Sprite betaSprite=null, Sprite gammaSprite=null)
    {
        _game.RemoveFromClassList("SingleTarget");
        _game.RemoveFromClassList("DoubleTarget");
        _game.RemoveFromClassList("TripleTarget");
        
        var targetIndex = 1;

        foreach (var card in _alphaTargetCards)
        {
            card.style.backgroundImage = new StyleBackground(alphaSprite);
        }
        if (betaSprite != null)
        {
            targetIndex=2;
            foreach (var card in _betaTargetCards)
            {
                card.style.backgroundImage = new StyleBackground(betaSprite);
            }
        }
        if (gammaSprite != null)
        {
            targetIndex=3;
            foreach (var card in _gammaTargetCards)
            {
                card.style.backgroundImage = new StyleBackground(gammaSprite);
            }
        }

        switch (targetIndex)
        {
            case 1:
                _game.AddToClassList("SingleTarget");
                break;
            case 2:
                _game.AddToClassList("DoubleTarget");
                break;
            case 3:
                _game.AddToClassList("TripleTarget");
                break;
        }
        
        // アンサーカードをクリア
        _footer.RemoveFromClassList("Successful");
        _answerCardContainer.Clear();
    }
    
    
    // アンサーカードを追加
    public void AddAnswerCard(Sprite sprite)
    {
        var card = new AnswerCard();
        card.Init(0.6f, sprite);
        _answerCardContainer.Add(card);
        _footer.AddToClassList("Successful");
    }
}
