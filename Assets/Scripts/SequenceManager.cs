using System;
using System.Collections.Generic;
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
    
    private string targetWard;
    
    private List<AnswerCardController> _answerCards = new ();

    public void CreateAnswerCard(AnswerCardController prefab, bool isSame)
    {
        var spawnPos = new Vector3(
            Random.Range(-_borderSize.x / 2, _borderSize.x / 2), 
            Random.Range(-_borderSize.y / 2, _borderSize.y / 2), 0);
        
        var card = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
        _answerCards.Add(card);
        
        card.onClick.Subscribe(_ =>
        {
            card.onAnswer.OnNext(isSame);
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
        
        CreateAnswerCard(correctProp.prefab, true);

        var cardNumbs = Random.Range(0, 5);
        for (int i = 0; i < cardNumbs; i++)
        {
            var ward = _wardViewAsset.GetRandomWard();
            var prop = _wardViewAsset.GetCorrectAnswerProp(ward);
            CreateAnswerCard(prop.prefab, ward == targetWard);
        }
    }

    private async void UpdateTarget()
    {
        targetWard = _wardViewAsset.GetRandomWard();
        
        GenerateAnswerCards();
        _gameUIManager.NextTarget();
        await UniTask.Delay(TimeSpan.FromSeconds(3));
        _gameUIManager.Reset();
        _gameUIManager.TimerStart(5);
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        _gameUIManager.GameOver();
        
    }
}
