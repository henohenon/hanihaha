using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using R3;
using UnityEditor.ShaderGraph;
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
    
    private Dictionary<AnswerCardController, bool> _answerCards = new ();
    
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
