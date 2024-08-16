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
    [SerializeField]
    private float _timerDefault = 10;
    private float _timer = 10;
    
    private int _sameCount = 0;
    private int _answerCount = 0;
    private string _targetWard;
    private bool _isHighScore = false;
    
    private MotionHandle _timerHandle; 
    private void Start()
    {
        _noGameUIManager.OnStart.Subscribe(_ =>
        {
            StartGame();
        }).AddTo(this);
        _noGameUIManager.OnRestart.Subscribe(_ =>
        {
            StartGame();
        }).AddTo(this);
        _noGameUIManager.OnReturn.Subscribe(_ =>
        {
            _screenUIManager.ChangeScreen(ScreenType.Start);
        }).AddTo(this);
        
        _answerCardsManager.OnAnswer.Subscribe(sprite =>
        {
            _gameUIManager.AddAnswerCard(sprite);
            _answerCount++;
            if (_answerCount == _sameCount)
            {
                AddTime(5);
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
            MinusTime(5);
        }).AddTo(this);
    }

    private void StartGame()
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
        _timerHandle = LMotion.Create(_timer, 0, _timer).WithOnComplete(() =>
        {
            _noGameUIManager.SetScore(_scoreManager.GetScore());
            _audioManager.SetIsPlayLimit(false);
            if (_isHighScore)
            {
                _scoreManager.SendScore();
                _isHighScore = false;
                _screenUIManager.ChangeScreen(ScreenType.HighScore);
            }
            else
            {
                _screenUIManager.ChangeScreen(ScreenType.GameOver);
            }
        }).BindWithState(_gameUIManager, (_time, _ui) =>
        {
            _timer = _time;
            _ui.SetTimer(_timer);
            var isLimit = _timer <= 5;
            _ui.SetLimit(isLimit);
            _audioManager.SetIsPlayLimit(isLimit);
        }).AddTo(this);
    }
    
    private void GenerateAnswerCards()
    {
        var correctProp = _wardViewAsset.GetCorrectAnswerProp(_targetWard);
        _answerCardsManager.CreateAnswerCard(correctProp.sprite, true);
        _sameCount++;
        
        var cardNumbs = Random.Range(5, 10);
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

    private async void UpdateTarget()
    {
        _timerHandle.PlaybackSpeed = 0;

        _answerCount = 0;
        _sameCount = 0;
        _answerCardsManager.ClearAnswerCards();
        _targetWard = _wardViewAsset.GetRandomWard();
        var questionProp = _wardViewAsset.GetCorrectAnswerProp(_targetWard);
        _gameUIManager.UpdateTarget(questionProp.sprite);
        _screenUIManager.ChangeScreen(ScreenType.NextTarget);
        _audioManager.PlayNextTargetSound();
        GenerateAnswerCards();
        
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        
        _timerHandle.PlaybackSpeed = 1;
        _screenUIManager.ChangeScreen(ScreenType.Game);
    }

    private void AddTime(int addTime)
    {
        _timer += addTime;
        ResetTimer();
        _gameUIManager.ShowPlusTime(addTime);
    }
    
    private void MinusTime(int minusTime)
    {
        _timer -= minusTime;
        ResetTimer();
        _gameUIManager.ShowMinusTime(minusTime);
    }
}
