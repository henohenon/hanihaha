using LitMotion;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenUIManager
{
    private VisualElement _body;
    private VisualElement _game;
    
    public ScreenUIManager(VisualElement root)
    {
        _body = root.Q<VisualElement>("Body");
        _game = root.Q<VisualElement>("Game");
    }
    
    public void ChangeScreen(ScreenType type)
    {
        _body.ClearClassList();
        
        switch (type)
        {
            case ScreenType.Title:
                _body.AddToClassList("Title");
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
    
    
    private bool _oldIsLimit = false;
    public void SetLimit(bool isLimit)
    {
        if (_oldIsLimit == isLimit) return;
        _oldIsLimit = isLimit;
        
        if (isLimit){
            _game.AddToClassList("Limit");
        }
        else
        {
            _game.RemoveFromClassList("Limit");
        }
    }
}

public enum ScreenType
{
    Title,
    NextTarget,
    Game,
    HighScore,
    GameOver
}