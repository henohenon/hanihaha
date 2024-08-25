using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Sprite))]
public class AnswerCardController : MonoBehaviour, IPointable
{
    public Subject<bool> onHover { get; } = new ();
    public Subject<Unit> onClick { get; } = new ();
    
    private bool _isAnswered = false;
    
    [SerializeField]
    private LayerMask _answerCardLayer;
    [SerializeField]
    private PolygonCollider2D _collider;
    [SerializeField]
    private MeshRenderer _renderer;

    [SerializeField] private AnimationCurve _sizeRandomCurve;

    protected Rigidbody2D _rb;
    
    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = Random.Range(-1f, 1f) * 0.3f;
        transform.localScale = Vector3.one * _sizeRandomCurve.Evaluate(Random.Range(0f, 1f));
    }
    
    public void Init(Sprite sprite)
    {
        _renderer.material.mainTexture = sprite.texture;

        _renderer.gameObject.transform.localScale = 
            new Vector3(
                sprite.rect.width / 500 * 1.25f, 
                sprite.rect.height / 500 * 1.25f,
                1
            );
        
        var physicsShapeCount = sprite.GetPhysicsShapeCount();

        _collider.pathCount = physicsShapeCount;

        var physicsShape = new List<Vector2>();

        for ( var i = 0; i < physicsShapeCount; i++ )
        {
            physicsShape.Clear();
            sprite.GetPhysicsShape( i, physicsShape );
            var points = physicsShape.ToArray();
            _collider.SetPath( i, points );
        }
        
        onHover.Subscribe(isHovered =>
        {
            if (_isAnswered) return;
            if (isHovered)
            {
                //_spriteRenderer.color = Color.gray;
            }
            else
            {
                //_spriteRenderer.color = Color.white;
            }
        }).AddTo(this);
        
        onClick.Subscribe(_ =>
        {
            //_spriteRenderer.enabled = false;
            _collider.enabled = false;
        }).AddTo(this);
    }

    private int hitCount = 0;
    private CancellationTokenSource cts;
    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == _answerCardLayer)
        {
            hitCount++;
            if (hitCount == 1)
            {
                WaitToTrigger();
            }
        }
    }

    private async UniTaskVoid WaitToTrigger()
    {
        // 既存のタスクがあればキャンセル
        cts?.Cancel();
        cts = new CancellationTokenSource();

        try
        {
            // 一定時間待機するが、キャンセルが可能
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: cts.Token);
            _collider.isTrigger = true;
        }
        catch (OperationCanceledException)
        {
            // タスクがキャンセルされたとき。tryは必ずcatchがいるのです。
        }
        cts.Dispose();
    }
    
    protected virtual void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == _answerCardLayer)
        {
            hitCount--;
            if (hitCount <= 0)
            {
                cts?.Cancel();
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == _answerCardLayer)
        {
            hitCount++;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == _answerCardLayer)
        {
            hitCount--;
            if (hitCount <= 0)
            {
                _collider.isTrigger = false;
            }
        }
    }

    private void OnDestroy()
    {
        cts?.Dispose();
    }
}


public interface IPointable
{
    public Subject<bool> onHover { get; }
    public Subject<Unit> onClick { get; }
}
