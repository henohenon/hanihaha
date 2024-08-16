using System.Collections.Generic;
using R3;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnswerCardController : MonoBehaviour, IPointable
{
    
    private SpriteRenderer _spriteRenderer;
    private PolygonCollider2D _collider;
    
    private static readonly Subject<Unit> sampleStaticSubject = new();

    public Subject<bool> onHover { get; } = new ();
    public Subject<Unit> onClick { get; } = new ();
    
    private bool _isAnswered = false;
    
    private Sprite _sprite;
    
    private void Start()
    {
        sampleStaticSubject.OnNext(Unit.Default);

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<PolygonCollider2D>();
        
        _spriteRenderer.sprite = _sprite;

        var physicsShapeCount = _sprite.GetPhysicsShapeCount();

        _collider.pathCount = physicsShapeCount;

        var physicsShape = new List<Vector2>();

        for ( var i = 0; i < physicsShapeCount; i++ )
        {
            physicsShape.Clear();
            _sprite.GetPhysicsShape( i, physicsShape );
            var points = physicsShape.ToArray();
            _collider.SetPath( i, points );
        }
        
        onHover.Subscribe(isHovered =>
        {
            if (_isAnswered) return;
            if (isHovered)
            {
                _spriteRenderer.color = Color.gray;
            }
            else
            {
                _spriteRenderer.color = Color.white;
            }
        }).AddTo(this);
        
        onClick.Subscribe(_ =>
        {
            _spriteRenderer.enabled = false;
            _collider.enabled = false;
        }).AddTo(this);
    }

    public void Init(Sprite sprite)
    {
        _sprite = sprite;
    }

    public void Answered(bool isSame)
    {
    }
}

public interface IPointable
{
    public Subject<bool> onHover { get; }
    public Subject<Unit> onClick { get; }
}
