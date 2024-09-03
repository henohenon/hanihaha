using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    private readonly float _timerDefault = 10;
    
    private ScreenUIManager _screenUIManager;
    private BackGroundManager _backGroundManager;
    private MeshCardsManager _meshCardsManager;
    private AudioManager _audioManager;
    private ScoreManager _scoreManager;
    private TimerManager _timerManager;
    private ComboManager _comboManager;
    private PointerSelectManager _pointerSelectManager;
    

    public void GoToTitle()
    {
        _screenUIManager.ChangeScreen(ScreenType.Title);
        _backGroundManager.ChangeScreen(ScreenType.Title);
        _meshCardsManager.ClearAnswerCards();
    }
    
    
    public void ReStartGame()
    {
        _audioManager.PlayGameStartSound();
        _timerManager.SetTime(_timerDefault);
        _scoreManager.ResetScore();
        _screenUIManager.ChangeScreen(ScreenType.Game);
        _backGroundManager.ChangeScreen(ScreenType.Game);
    }

    public void GameEnd()
    {
        _pointerSelectManager.IsCanSelect = false;
        _audioManager.SetIsPlayLimit(false);
        _backGroundManager.SetLimit(false);
        _meshCardsManager.ShowResults();
    }
    
    public void HighScore()
    {
        _audioManager.PlayHighScoreSound();
        _scoreManager.SendScore();
        _backGroundManager.ChangeScreen(ScreenType.HighScore);
        _screenUIManager.ChangeScreen(ScreenType.HighScore);
    }
    public void GameOver()
    {
        _audioManager.PlayGameOverSound();
        _backGroundManager.ChangeScreen(ScreenType.GameOver);
        _screenUIManager.ChangeScreen(ScreenType.GameOver);
    }
    
    
    private int _sameCount = 0;
    private int _answerCount = 0;
    private Sprite _target;
    public void ForNextTarget()
    {
        _pointerSelectManager.IsCanSelect = false;
        _timerManager.SetPause(true);
        
        // 値をクリア
        _answerCount = 0;
        _sameCount = 0;
        _meshCardsManager.ClearAnswerCards();

        // 新ターゲット
        _target = _spriteViewAsset.GetRandom();
        // UI更新
        _gameUIManager.UpdateTarget(_target);
        _screenUIManager.ChangeScreen(ScreenType.NextTarget);
        _backGroundManager.ChangeScreen(ScreenType.NextTarget);
        // アンサーカード生成
        GenerateAnswerCards();
    }

    public void StartTarget()
    {   
        _timerManager.SetPause(false);
        _screenUIManager.ChangeScreen(ScreenType.Game);
        _backGroundManager.ChangeScreen(ScreenType.Game);
        _comboManager.ResetCombo();
        _audioManager.PlayNextTargetSound();
        _pointerSelectManager.IsCanSelect = true;
    }
    
}
