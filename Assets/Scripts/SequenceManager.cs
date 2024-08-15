using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
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
    private AudioManager _audioManager;
    [SerializeField]
    private ScoreManager _scoreManager;
    [SerializeField]
    private float _timer = 10;
    
    private string _targetWard;
    
    private void Start()
    {
        UpdateTarget();
    }
    
    private void GenerateAnswerCards()
    {
        var correctProp = _wardViewAsset.GetCorrectAnswerProp(_targetWard);
        
        CreateAnswerCard(correctProp, true);

        var cardNumbs = Random.Range(0, 5);
        for (int i = 0; i < cardNumbs; i++)
        {
            var ward = _wardViewAsset.GetRandomWard();
            var prop = _wardViewAsset.GetCorrectAnswerProp(ward);
            CreateAnswerCard(prop, ward == _targetWard);
        }
    }

    private async void UpdateTarget()
    {
        _answerCardsManager.ClearAnswerCards();
        _targetWard = _wardViewAsset.GetRandomWard();
        var questionProp = _wardViewAsset.GetCorrectAnswerProp(_targetWard);
        _gameUIManager.UpdateTarget(questionProp.sprite);
        _screenUIManager.ChangeScreen(ScreenType.NextTarget);
        _audioManager.PlayNextTargetSound();
        
        GenerateAnswerCards();
        gameUIManager.NextTarget(questionProp.sprite);
        gameUIManager.SetIsLimit(false);
        _audioManager.PlayNextTargetSound();
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        gameUIManager.Reset();
        _waiting = false;
        gameUIManager.TimerStart(5);
        await UniTask.Delay(TimeSpan.FromSeconds(2));
        if (!_successful)
        {
            _audioManager.SetIsPlayLimit(true);
            gameUIManager.SetIsLimit(true);
        }
        await UniTask.Delay(TimeSpan.FromSeconds(3));
        if (!_successful)
        {
            _scoreManager.SendScore(_score);
            gameUIManager.GameOver();
        }
        else
        {
            _score++;
            UpdateTarget();
        }
        _audioManager.SetIsPlayLimit(false);
    }
}
