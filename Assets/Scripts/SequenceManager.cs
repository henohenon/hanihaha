using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

public class SequenceManager : MonoBehaviour
{
    [SerializeField]
    private WardViewAsset _wardViewAsset;
    [SerializeField]
    private Transform _borderObj;
    [SerializeField]
    private GameUIManager _gameUIManager;
    [SerializeField]
    private AudioManager _audioManager;
    [SerializeField]
    private ScoreManager _scoreManager;
    [AssetsOnly]
    [SerializeField]
    private AnswerCardController prefab;
    
    
    private string targetWard;
    private bool _successful;
    private bool _waiting = true;
    
    private int _score = 0;
    
    private List<AnswerCardController> _answerCards = new ();

    private void RefreshAnswerCards()
    {
        foreach (var card in _answerCards)
        {
            Destroy(card.gameObject);
        }
        _answerCards.Clear();
    }
    
    private void CreateAnswerCard(AnswerCardProp prop, bool isSame)
    {
        var spawnPos = new Vector3(
            Random.Range(-_borderSize.x / 2, _borderSize.x / 2), 
            Random.Range(-_borderSize.y / 2, _borderSize.y / 2), 0);
        
        var card = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
        card.Init(prop.sprite);
        _answerCards.Add(card);
        
        card.onClick.Subscribe(_ =>
        {
            if (_waiting) return;
            card.onAnswer.OnNext(isSame);
        }).AddTo(card);
        
        card.onAnswer.Subscribe(isCorrect =>
        {
            if (_waiting) return;
            if (isCorrect)
            {
                _gameUIManager.AddAnswerCard();
                _successful = true;
                _audioManager.PlaySuccessEffect();
                _audioManager.SetIsPlayLimit(false);
            }
            else
            {
                _audioManager.PlayFailEffect();
            }
        }).AddTo(card);
    }
    
    
    private Vector2 _borderSize;
    
        
    private void ResetBorderSize()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        
        _borderSize = new Vector2(width, height);

        _borderObj.localScale = new Vector3(_borderSize.x, _borderSize.y, 1);
    }
    private void Start()
    {
        ResetBorderSize();
        UpdateTarget();
        
    }
    
    private void GenerateAnswerCards()
    {
        var correctProp = _wardViewAsset.GetCorrectAnswerProp(targetWard);
        
        CreateAnswerCard(correctProp, true);

        var cardNumbs = Random.Range(0, 5);
        for (int i = 0; i < cardNumbs; i++)
        {
            var ward = _wardViewAsset.GetRandomWard();
            var prop = _wardViewAsset.GetCorrectAnswerProp(ward);
            CreateAnswerCard(prop, ward == targetWard);
        }
    }

    private async void UpdateTarget()
    {
        targetWard = _wardViewAsset.GetRandomWard();
        
        _successful = false;
        RefreshAnswerCards();
        _waiting = true;
        _gameUIManager.NextTarget();
        _gameUIManager.SetIsLimit(false);
        GenerateAnswerCards();
        _audioManager.PlayStarGameEffect();
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        _gameUIManager.Reset();
        _waiting = false;
        _gameUIManager.TimerStart(5);
        await UniTask.Delay(TimeSpan.FromSeconds(2));
        if (!_successful)
        {
            _audioManager.SetIsPlayLimit(true);
            _gameUIManager.SetIsLimit(true);
        }
        await UniTask.Delay(TimeSpan.FromSeconds(3));
        if (!_successful)
        {
            _scoreManager.SendScore(_score);
            _gameUIManager.GameOver();
        }
        else
        {
            _score++;
            UpdateTarget();
        }
        _audioManager.SetIsPlayLimit(false);
    }
}
