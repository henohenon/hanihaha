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
    private float _timerDefault = 10;

    [SerializeField]
    private SpriteViewAsset _spriteViewAsset;
    [SerializeField]
    private AnswerCardsManager _answerCardsManager;
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
    
    private int _sameCount = 0;
    private int _answerCount = 0;
    private Sprite _target;
    private bool _isHighScore = false;
    
    private DifficultyLevel _currentLevel;
    
    private float lastAnswerTime;
    private int comboCount = 0;
    
    private void Start()
    {
        _noGameUIManager.OnStart.Subscribe(_ =>
        {
            ReStartGame();
        }).AddTo(this);
        _noGameUIManager.OnRestart.Subscribe(_ =>
        {
            ReStartGame();
        }).AddTo(this);
        _noGameUIManager.OnReturn.Subscribe(_ =>
        {
            GoToTitle();
        }).AddTo(this);
        
        _answerCardsManager.OnAnswer.Subscribe(sprite =>
        {
            _gameUIManager.AddAnswerCard(sprite);
            _answerCount++;
            _audioManager.PlaySuccessSound();
            
            
            var deltaTime = Time.time - lastAnswerTime;
            if (comboCount == -1 || deltaTime < 0.5f)
            {
                comboCount+=2;
                if (comboCount != 0)
                {
                    _answerCardsManager.AddComboCard(comboCount);
                    _timerManager.AddTime(GetComboAddTime(comboCount));
                }
            }
            lastAnswerTime = Time.time;
            if (_answerCount == _sameCount)
            {
                _timerManager.AddTime(_currentLevel.eachPlusTime);
                _isHighScore = _scoreManager.AddScore();
                UpdateTarget();
            }
            
        }).AddTo(this);
        _answerCardsManager.OnFailure.Subscribe(_ =>
        {
            comboCount = -1;
            _audioManager.PlayFailSound();
            _timerManager.MinusTime(_currentLevel.eachMinusTime);
        }).AddTo(this);
        
        _scoreManager.OnLevelUpdate.Subscribe(level =>
        {
            _currentLevel = level;
        }).AddTo(this);
        
        _timerManager.OnTimeUp.Subscribe(_ =>
        {            
            _pointerSelectManager.IsCanSelect = false;
            _noGameUIManager.SetScore(_scoreManager.GetScore());
            _audioManager.SetIsPlayLimit(false);
            if (_isHighScore)
            {
                _audioManager.PlayHighScoreSound();
                _scoreManager.SendScore();
                _isHighScore = false;
                _screenUIManager.ChangeScreen(ScreenType.HighScore);
            }
            else
            {
                _audioManager.PlayGameOverSound();
                _screenUIManager.ChangeScreen(ScreenType.GameOver);
            }

        }).AddTo(this);
    }
    
    private void GoToTitle()
    {
        _screenUIManager.ChangeScreen(ScreenType.Title);
    }

    private void ReStartGame()
    {
        _timerManager.SetTime(_timerDefault);
        _scoreManager.ResetScore();
        _screenUIManager.ChangeScreen(ScreenType.Game);
        UpdateTarget();
    }
    
    private void GenerateAnswerCards()
    {
        var cardNumbs = Random.Range(_currentLevel.minCardNum, _currentLevel.maxCardNum);
        for (int i = 0; i < cardNumbs; i++)
        {
            var sprite = _spriteViewAsset.GetRandom();
            var isSame = _spriteViewAsset.IsSame(_target, sprite);
            _answerCardsManager.CreateAnswerCard(sprite, isSame);
            
            if (isSame)
            {
                _sameCount++;
            }

            if (_sameCount <= 0 && i + 1 >= cardNumbs)
            {
                i--;
            }
        }
    }
    
    private void ForNextTarget()
    {
        _pointerSelectManager.IsCanSelect = false;
        _timerManager.SetPause(true);
        
        // 値をクリア
        _answerCount = 0;
        _sameCount = 0;
        _answerCardsManager.ClearAnswerCards();

        // 新ターゲット
        _target = _spriteViewAsset.GetRandom();
        // UI更新
        _gameUIManager.UpdateTarget(_target);
        _screenUIManager.ChangeScreen(ScreenType.NextTarget);
        _audioManager.PlayNextTargetSound();
        // アンサーカード生成
        GenerateAnswerCards();
    }

    private async void UpdateTarget()
    {
        ForNextTarget();
     
        await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
        
        _timerManager.SetPause(false);
        _screenUIManager.ChangeScreen(ScreenType.Game);
        lastAnswerTime = Time.time;
        comboCount = -1;
        _pointerSelectManager.IsCanSelect = true;
    }

    private float GetComboAddTime(int comboCount)
    {
        switch (comboCount)
        {
            case 0:
                return 0;
            case 1:
                return 0.1f;
            case 2:
                return 0.1f;
            case 3:
                return 0.2f;
            case 4:
                return 0.2f;
            case 5:
                return 0.3f;
            case 6:
                return 0.3f;
            case 7:
                return 0.5f;
            case 8:
                return 0.5f;
            case 9:
                return 0.5f;
            default:
                return 0.5f + comboCount-0.9f * 0.1f;

        }
    }
}
