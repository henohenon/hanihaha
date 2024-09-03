using System;
using R3;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class NoGameUIManager
{
    private Button _startButton;
    private Button _restartButton;
    private Button _returnButton;
    private TextElement _scoreLabel;

    private Subject<Unit> _onStart = new ();
    private Subject<Unit> _onRestart = new ();
    private Subject<Unit> _onReturn = new ();
    
    public Observable<Unit> OnStart => _onStart;
    public Observable<Unit> OnRestart => _onRestart;
    public Observable<Unit> OnReturn => _onReturn;
    
    public NoGameUIManager(VisualElement root)
    {
        _startButton = root.Q<Button>("StartButton");
        _restartButton = root.Q<Button>("RestartButton");
        _returnButton = root.Q<Button>("ReturnButton");
        _scoreLabel = root.Q<TextElement>("Score");
        
        _startButton.clicked += () =>
        { 
            _onStart.OnNext(Unit.Default);
        };
        _restartButton.clicked += () =>
        {
            _onRestart.OnNext(Unit.Default);
        };
        _returnButton.clicked += () =>
        {
            _onReturn.OnNext(Unit.Default);
        };
    }
    
    public void SetScore(int score)
    {
        _scoreLabel.text = "Score " + score.ToString();
    }
}
