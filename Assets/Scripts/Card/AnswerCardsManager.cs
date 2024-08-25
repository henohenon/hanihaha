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
    private AnswerCardController[] answerCardPrefabs;
    [AssetsOnly]
    [SerializeField]
    private TextMesh comboTextPrefab;
    [SerializeField]
    private Transform _borderObj;
    
    private List<AnswerCardController> _answerCards = new ();
    
    private Vector2 _spawnSize;
    private Vector2 _borderSize;
    private Vector3 _lastSelectedPos;

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
        var randomRot = Random.Range(0, 360);
        
        var randomIndex = Random.Range(0, answerCardPrefabs.Length);
        var card = Instantiate(answerCardPrefabs[randomIndex], spawnPos, Quaternion.Euler(0, 0, randomRot), transform);
        card.Init(sprite);
        
        _answerCards.Add(card);
        
        // クリック時の処理
        card.onClick.Subscribe(_ =>
        {
            _lastSelectedPos = card.transform.position;
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

    public void AddComboCard(int comboIndex)
    {
        var instancePos = _lastSelectedPos;
        instancePos.z = 1;
        var comboCardInstance = Instantiate(comboTextPrefab, instancePos, Quaternion.identity, transform);
        new ComboCardController(comboIndex, comboCardInstance);
    }
}
