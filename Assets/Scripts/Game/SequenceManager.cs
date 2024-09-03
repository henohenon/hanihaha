using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using LitMotion;
using R3;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SequenceManager : MonoBehaviour
{
    [SerializeField]
    private SpriteViewAsset _spriteViewAsset;
    [FormerlySerializedAs("_answerCardsManager")] [SerializeField]
    private MeshCardsManager meshCardsManager;
    [SerializeField]
    private GameUIManager _gameUIManager;
    [SerializeField]
    private ScreenUIManager _screenUIManager;
    [SerializeField]
    private NoGameUIManager _noGameUIManager;
    [SerializeField]
    private AudioManager _audioManager;
    [SerializeField]
    private ScoreManager _scoreManager;
    [SerializeField]
    private TimerManager _timerManager;
    [SerializeField] 
    private PointerSelectManager _pointerSelectManager;
    [SerializeField]
    private BackGroundManager _backGroundManager;
    
    private ScreenManager _screenManager;
    private ComboManager _comboManager;
    
    private int _sameCount = 0;
    private int _answerCount = 0;
    private Sprite _target;
    private bool _isHighScore = false;
    
    private DifficultyLevel _currentLevel;
    
    
    private void Start()
    {
        _noGameUIManager.OnStart.Subscribe(_ =>
        {
            _screenManager.ReStartGame();
        }).AddTo(this);
        _noGameUIManager.OnRestart.Subscribe(_ =>
        {
            _screenManager.ReStartGame();
        }).AddTo(this);
        _noGameUIManager.OnReturn.Subscribe(_ =>
        {
            _screenManager.GoToTitle();
        }).AddTo(this);
        
        meshCardsManager.OnAnswer.Subscribe( sprite =>
        {
            _gameUIManager.AddAnswerCard(sprite);
            _answerCount++;

            var isCombo = _comboManager.OnAnswer();
            _audioManager.PlaySuccessSound(isCombo);
            
            if (_answerCount == _sameCount)
            {
                _timerManager.AddTime(_currentLevel.eachPlusTime);
                _isHighScore = _scoreManager.AddScore();
                ChangeTargetUntilWait();
            }
        }).AddTo(this);
        meshCardsManager.OnFailure.Subscribe(_ =>
        {
            _comboManager.ResetCombo();
            _audioManager.PlayFailSound();
            _timerManager.MinusTime(_currentLevel.eachMinusTime);
        }).AddTo(this);
        
        _scoreManager.OnLevelUpdate.Subscribe(level =>
        {
            _currentLevel = level;
        }).AddTo(this);
        
        _timerManager.OnTimeUp.Subscribe(_ =>
        {
            _screenManager.GameEnd();
            if (_isHighScore)
            {
                _screenManager.HighScore();
                _isHighScore = false;
            }
            else
            {
                _screenManager.GameOver();
            }
        }).AddTo(this);
    }

    private async void ChangeTargetUntilWait()
    {
        _screenManager.ForNextTarget();
        await UniTask.Delay(TimeSpan.FromSeconds(1.3f));
        _screenManager.StartTarget();
    }

}
