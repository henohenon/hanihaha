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
    private VisualElement _targetCard;
    private VisualElement _answerCardContainer;

    [SerializeField] private VisualTreeAsset _answerCard;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        
        _timerLabel = _uiDocument.rootVisualElement.Q<Label>("Time");
        _targetCard = _uiDocument.rootVisualElement.Q<VisualElement>("Target");
        _answerCardContainer = _uiDocument.rootVisualElement.Q<VisualElement>("AnswerCards");
        
        // GeometryChangedEventを登録
        _targetCard.RegisterCallback<GeometryChangedEvent>(evt =>
        {
            SetWidthByHeight(_targetCard);
        });
    }

    [Button]
    public async void TimerStart(float time = 3)
    {
        LMotion.Create(time, 0f, time)
            .BindWithState(_timerLabel, (x, label) => label.text = x.ToString("F2"));
        if (time <= 3)
        {
            _timerLabel.AddToClassList("ThreeSeconds");
        }
        else
        {
            _timerLabel.RemoveFromClassList("ThreeSeconds");
            await UniTask.WaitForSeconds(time - 3);
            _timerLabel.AddToClassList("ThreeSeconds");
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
    }

    [Button]
    public void ResetAnswer()
    {
        _answerCardContainer.Clear();
    }

    
    private void SetWidthByHeight(VisualElement element, float rate = 1)
    {
        element.style.width = element.resolvedStyle.height * rate;
    }
}
