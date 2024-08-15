using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using LitMotion;
using R3;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


[RequireComponent(typeof(UIDocument))]
public class GameUIManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    
    private List<VisualElement> _targetContainers;
    private List<VisualElement> _targetCards;
    private Label _timerLabel;
    private TextElement _minusTime;
    private TextElement _plusTime;
    private VisualElement _answerCardContainer;

    [SerializeField] private VisualTreeAsset _answerCard;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        
        _targetContainers = _uiDocument.rootVisualElement.Query<VisualElement>(classes:"TargetContainer").ToList();
        _targetCards = new List<VisualElement>();
        _timerLabel = _uiDocument.rootVisualElement.Q<Label>("Time");
        _minusTime = _uiDocument.rootVisualElement.Q<TextElement>("MinusTime");
        _plusTime = _uiDocument.rootVisualElement.Q<TextElement>("PlusTime");
        _answerCardContainer = _uiDocument.rootVisualElement.Q<VisualElement>("AnswerCards");
        
        // あらかじめターゲットカードを取得しておく
        foreach (var targetContainer in _targetContainers)
        {
            var targetCard = targetContainer.Query<VisualElement>(classes: "AnswerCard").ToList();
            _targetCards.AddRange(targetCard);
            
            // 高さに合わせて幅を設定
            targetContainer.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                SetWidthByHeight(targetContainer, 1);
            });
        }
    }
    
    // タイマーを設定
    public void SetTimer(float time)
    {
        _timerLabel.text = time.ToString("F2");
    }
    
    // アンサーカードを追加
    public void AddAnswerCard(Sprite sprite)
    {
        var card = _answerCard.CloneTree();
        var cardImg = card.Q<VisualElement>(classes: "AnswerCard");
        cardImg.style.backgroundImage = new StyleBackground(sprite);
        _answerCardContainer.Add(card);

        // 高さに合わせて幅を設定
        void OnGeometryChanged(GeometryChangedEvent evt)
        {
            SetWidthByHeight(card, 0.6f);
            card.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }
        card.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    // ターゲットカードを更新
    public void UpdateTarget(Sprite sprite)
    {
        // アンサーカードをクリア
        _answerCardContainer.Clear();
        
        var background = new StyleBackground(sprite);
        foreach (var targetCard in _targetCards)
        {
            targetCard.style.backgroundImage = background;
        }
    }
    
    // 高さに合わせて幅を設定
    private void SetWidthByHeight(VisualElement element, float rate = 1)
    {
        element.style.width = element.resolvedStyle.height * rate;
    }
    
    // プラスのタイマーを表示
    private CancellationTokenSource _plusTimeCts;
    public void ShowPlusTime(float time)
    {
        ShowTimer(time, _plusTime, _plusTimeCts);
    }

    // マイナスのタイマーを表示
    private CancellationTokenSource _minusTimeCts;
    public void ShowMinusTime(float time)
    {
        ShowTimer(time, _minusTime, _minusTimeCts);
    }

    // キャンセル可能なタイマー表示
    private async void ShowTimer(float time, TextElement elem, CancellationTokenSource _cts)
    {
        
        // 既存のタスクがあればキャンセル
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        try
        {
            elem.text = "+" + time.ToString("F2");
            elem.style.display = DisplayStyle.Flex;

            // 一定時間待機するが、キャンセルが可能
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: _cts.Token);

            elem.style.display = DisplayStyle.None;
        }
        catch (OperationCanceledException)
        {
            // タスクがキャンセルされたとき。tryは必ずcatchがいるのです。
        }
        _cts.Dispose();
    }

    private void OnDestroy()
    {
        _plusTimeCts?.Dispose();
        _minusTimeCts?.Dispose();
    }
}
