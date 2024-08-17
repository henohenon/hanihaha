using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnswerCardsManager : MonoBehaviour
{
    [AssetsOnly]
    [SerializeField]
    private AnswerCardController prefab;
    [SerializeField]
    private Transform _borderObj;
    
    private List<AnswerCardController> _answerCards = new ();
    
    private Vector2 _spawnSize;
    private Vector2 _borderSize;

    private Subject<Sprite> _onAnswer = new ();
    private Subject<Sprite> _onFialur = new ();
    public Observable<Sprite> OnAnswer => _onAnswer;
    public Observable<Sprite> OnFailure => _onFialur;
    
    private void Start()
    {
        ResetBorderSize();
    }

    // ボーダー系のリセット
    private void ResetBorderSize()
    {
        var cam = Camera.main;
        var height = 2f * cam.orthographicSize;
        var width = height * cam.aspect;
        
        _borderSize = new Vector2(width, height);
        _spawnSize = new Vector2(width - 1, height - 1);
        _borderObj.localScale = new Vector3(_borderSize.x, _borderSize.y, 1);
    }
    
    // カードを削除
    public void ClearAnswerCards()
    {
        foreach (var card in _answerCards)
        {
            Destroy(card.gameObject);
        }
        _answerCards.Clear();
    }
    
    // カードを追加
    public void CreateAnswerCard(Sprite sprite, bool isSame)
    {
        // ランダムな位置
        var spawnPos = new Vector3(
            Random.Range(-_spawnSize.x / 2, _spawnSize.x / 2), 
            Random.Range(-_spawnSize.y / 2, _spawnSize.y / 2), 0);
        
        var card = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
        card.Init(sprite);
        _answerCards.Add(card);
        
        // クリック時の処理
        card.onClick.Subscribe(_ =>
        {
            if (isSame)
            {
                _onAnswer.OnNext(sprite);
            }
            else
            {
                _onFialur.OnNext(sprite);
            }
        }).AddTo(card);
    }

}
