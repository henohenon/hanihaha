
using LitMotion;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ScreenUIManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    
    private VisualElement _body;
    
    private void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        
        _body = _uiDocument.rootVisualElement.Q<VisualElement>("Body");
    }
    
    public void ChangeScreen(ScreenType type)
    {
        _body.ClearClassList();
        
        switch (type)
        {
            case ScreenType.Start:
                _body.AddToClassList("Start");
                break;
            case ScreenType.NextTarget:
                _body.AddToClassList("NextTarget");
                break;
            case ScreenType.Game:
                _body.AddToClassList("Game");
                break;
            case ScreenType.HighScore:
                _body.AddToClassList("GameOver");
                _body.AddToClassList("HighScore");
                break;
            case ScreenType.GameOver:
                _body.AddToClassList("GameOver");
                break;
        }
    }
}

public enum ScreenType
{
    Start,
    NextTarget,
    Game,
    HighScore,
    GameOver
}