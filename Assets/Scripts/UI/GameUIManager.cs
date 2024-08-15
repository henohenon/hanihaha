using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(UIDocument))]
public class GameUIManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Label _timerLabel;
    private List<VisualElement> _targetContainers;
    private List<VisualElement> _targetCards;
    private VisualElement _answerCardContainer;
    private VisualElement _body;
    

    [SerializeField] private VisualTreeAsset _answerCard;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        
        _timerLabel = _uiDocument.rootVisualElement.Q<Label>("Time");
        _targetContainers = _uiDocument.rootVisualElement.Query<VisualElement>(classes:"TargetContainer").ToList();
        _targetCards = new List<VisualElement>();
        _answerCardContainer = _uiDocument.rootVisualElement.Q<VisualElement>("AnswerCards");
        _body = _uiDocument.rootVisualElement.Q<VisualElement>("Body");
        
        foreach (var targetContainer in _targetContainers)
        {
            var targetCard = targetContainer.Query<VisualElement>(classes: "AnswerCard").ToList();
            _targetCards.AddRange(targetCard);
            
            targetContainer.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                SetWidthByHeight(targetContainer, 1);
            });
        }
    }
    
    [Button]
    public async void TimerStart(float time = 3)
    {
        LMotion.Create(time, 0f, time)
            .BindWithState(_timerLabel, (x, label) => label.text = x.ToString("F2"));
        if (time <= 3)
        {
            _body.AddToClassList("ThreeTime");
        }
        else
        {
            _body.RemoveFromClassList("ThreeTime");
            await UniTask.WaitForSeconds(time - 3);
            _body.AddToClassList("ThreeTime");
        }
    }
    
    [Button]
    public void AddAnswerCard()
    {
        var card = _answerCard.CloneTree();
        _answerCardContainer.Add(card);
        card.RegisterCallback<GeometryChangedEvent>(evt =>
        {
            SetWidthByHeight(card, 0.6f);
        });
        
        _body.AddToClassList("Successful");
    }

    [Button]
    public void Reset()
    {
        _answerCardContainer.Clear();
        
        _body.RemoveFromClassList("NextTarget");
        _body.RemoveFromClassList("ThreeTime");
        _body.RemoveFromClassList("Successful");
        _body.RemoveFromClassList("GameOver");
    }
    
    [Button]
    public void NextTarget()
    {
        /*
        var background = new StyleBackground(sprite);
        foreach (var targetCard in _targetCards)
        {
            targetCard.style.backgroundImage = background;
        }*/
        
        _body.AddToClassList("NextTarget");
    }
    
    [Button]
    public void GameOver()
    {
        _body.AddToClassList("GameOver");
    }

    
    private void SetWidthByHeight(VisualElement element, float rate = 1)
    {
        element.style.width = element.resolvedStyle.height * rate;
    }
}
