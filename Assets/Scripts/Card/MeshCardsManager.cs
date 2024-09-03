using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using R3;
using UnityEditor.ShaderGraph;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeshCardsManager : MonoBehaviour
{
    [SerializeField]
    private Transform _borderObj;
    
    private Dictionary<MeshCardController, bool> _answerCards = new ();
    
    private Vector2 _spawnSize;
    private Vector2 _borderSize;
    private MeshCardGenerator _cardGenerator;

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
        height -= 3;
        
        _borderSize = new Vector2(width, height);
        _spawnSize = new Vector2(width - 3, height - 3);
        _borderObj.localScale = new Vector3(_borderSize.x, _borderSize.y, 1);
    }
    
    // カードを削除
    public void ClearAnswerCards()
    {
        foreach (var card in _answerCards)
        {
            Destroy(card.Key.gameObject);
        }
        _answerCards.Clear();
        _isResulting = false;
    }
    
    private bool _isResulting = false;
    private Color _successColor = Color.HSVToRGB(0.47f, 0.7f, 1);
    private Color _failureColor = Color.HSVToRGB(0.02f, 0.7f, 1);

    public void ShowResults()
    {
        _isResulting = true;
        foreach (var card in _answerCards)
        {
            if(card.Value)
            {
                card.Key.ChangeColor(_successColor);
            }
            else
            {
                card.Key.ChangeColor(_failureColor);
            }
        }
    }

    public Vector3 GetNewCardPosition()
    {
        // ランダムな位置
        var spawnPos = new Vector3(
            Random.Range(-_spawnSize.x / 2, _spawnSize.x / 2),
            Random.Range(-_spawnSize.y / 2, _spawnSize.y / 2), 0);

        // 近くにカードがある場合は再生成。最大5回
        for (var i = 0; i < 5; i++)
        {
            var _notNear = false;
            foreach (var _card in _answerCards)
            {
                if (Vector3.Distance(_card.Key.transform.position, spawnPos) < 1.5f)
                {
                    _notNear = true;
                    break;
                }

            }

            if (_notNear)
            {
                break;
            }

            spawnPos = new Vector3(
                Random.Range(-_spawnSize.x / 2, _spawnSize.x / 2),
                Random.Range(-_spawnSize.y / 2, _spawnSize.y / 2), 0);
        }

        return spawnPos;
    }

    // カードを追加
    public void CreateAnswerCard(Sprite sprite, bool isSame)
    {
        var card = _cardGenerator.GenerateCard(sprite);
        _answerCards.Add(card, isSame);
        
        card.onHover.Subscribe(isHover =>
        {
            if (_isResulting) return;
            if (isHover)
            {
                card.ChangeColor(Color.gray);
            }
            else
            {
                card.ChangeColor(Color.white);
            }
        }).AddTo(card);
        // クリック時の処理
        card.onClick.Subscribe(_ =>
        {
            if (_isResulting) return;
            if (isSame)
            {
                _onAnswer.OnNext(sprite);
            }
            else
            {
                _onFialur.OnNext(sprite);
            }
            _answerCards.Remove(card);
            Destroy(card.gameObject);
        }).AddTo(card);
    }
}
