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
    private WardViewAsset _wardViewAsset;
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
    private float _timer = 10;
    
    private int _sameCount = 0;
    private int _answerCount = 0;
    private string _targetWard;
    private bool _isHighScore = false;
    
    private DifficultyLevel _currentLevel;
    
    private MotionHandle _timerHandle; 
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
            if (_answerCount == _sameCount)
            {
                AddTime(_currentLevel.eachPlusTime);
                _isHighScore = _scoreManager.AddScore();
                UpdateTarget();
            }
            else
            {
                _audioManager.PlaySuccessSound();
            }
        }).AddTo(this);
        _answerCardsManager.OnFailure.Subscribe(_ =>
        {
            _audioManager.PlayFailSound();
            MinusTime(_currentLevel.eachMinusTime);
        }).AddTo(this);
        
        _scoreManager.OnLevelUpdate.Subscribe(level =>
        {
            _currentLevel = level;
        }).AddTo(this);
    }
    
    private void GoToTitle()
    {
        _screenUIManager.ChangeScreen(ScreenType.Title);
    }

    private void ReStartGame()
    {
        _timer = _timerDefault;
        _scoreManager.ResetScore();
        _screenUIManager.ChangeScreen(ScreenType.Game);
        ResetTimer();
        UpdateTarget();
    }
    
    private void ResetTimer()
    {
        if (_timerHandle.IsActive())
        {
            _timerHandle.Cancel();
        }
        
        
        var isLimit = _timer <= 5;
        _audioManager.SetIsPlayLimit(isLimit);
        _gameUIManager.SetLimit(isLimit);
        
        _timerHandle = LMotion.Create(_timer, 0, _timer).WithOnComplete(() =>
        {
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
        }).BindWithState(_gameUIManager, (_time, _ui) =>
        {
            _timer = _time;
            _ui.SetTimer(_timer);
            var isLimit = _timer <= 5;
            _audioManager.SetIsPlayLimit(isLimit);
            _ui.SetLimit(isLimit);
        }).AddTo(this);
    }
    
    private void GenerateAnswerCards()
    {
        var correctProp = _wardViewAsset.GetCorrectAnswerProp(_targetWard);
        _answerCardsManager.CreateAnswerCard(correctProp.sprite, true);
        _sameCount++;
        
        var cardNumbs = Random.Range(_currentLevel.minCardNum, _currentLevel.maxCardNum);
        for (int i = 0; i < cardNumbs; i++)
        {
            var ward = _wardViewAsset.GetRandomWard();
            var prop = _wardViewAsset.GetCorrectAnswerProp(ward);
            var isSame = ward == _targetWard;
            _answerCardsManager.CreateAnswerCard(prop.sprite, isSame);
            
            if (isSame)
            {
                _sameCount++;
            }
        }
    }
    
    private void ForNextTarget()
    {
        _timerHandle.PlaybackSpeed = 0;

        // 値をクリア
        _answerCount = 0;
        _sameCount = 0;
        _answerCardsManager.ClearAnswerCards();

        // 新ターゲット
        _targetWard = _wardViewAsset.GetRandomWard();
        var questionProp = _wardViewAsset.GetCorrectAnswerProp(_targetWard);
        // UI更新
        _gameUIManager.UpdateTarget(questionProp.sprite);
        _screenUIManager.ChangeScreen(ScreenType.NextTarget);
        _audioManager.PlayNextTargetSound();
        // アンサーカード生成
        GenerateAnswerCards();
    }

    private async void UpdateTarget()
    {
        ForNextTarget();
        
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        
        _timerHandle.PlaybackSpeed = 1;
        _screenUIManager.ChangeScreen(ScreenType.Game);
    }

    private void AddTime(float addTime)
    {
        _timer += addTime;
        ResetTimer();
        _gameUIManager.ShowPlusTime(addTime);
    }
    
    private void MinusTime(float minusTime)
    {
        _timer -= minusTime;
        ResetTimer();
        _gameUIManager.ShowMinusTime(minusTime);
    }
}
