using System.Collections.Generic;
using R3;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnswerCardController : MonoBehaviour, ISelectable
{
    
    private SpriteRenderer _spriteRenderer;
    private PolygonCollider2D _collider;
    
    private static readonly Subject<Unit> sampleStaticSubject = new();

    public Subject<bool> onHover { get; } = new ();
    public Subject<Unit> onClick { get; } = new ();
    public Subject<bool> onAnswer { get; } = new ();
    
    private Observable<Unit> OnClick => onClick;
    
    private bool _isAnswered = false;
    
    private void Start()
    {
        sampleStaticSubject.OnNext(Unit.Default);

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<PolygonCollider2D>();
        var sprite = _spriteRenderer.sprite;

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
                _spriteRenderer.color = Color.gray;
            }
            else
            {
                _spriteRenderer.color = Color.white;
            }
        }).AddTo(this);
        
        onAnswer.Subscribe(isCorrect =>
        {
            if (isCorrect)
            {
                _spriteRenderer.color = new Color(14/255, 255/255, 0);
            }
            else
            {
                _spriteRenderer.color = Color.red;
            }
            _isAnswered = true;
        }).AddTo(this);
    }
    
}

public interface ISelectable
{
    public Subject<bool> onHover { get; }
    public Subject<Unit> onClick { get; }
    public Subject<bool> onAnswer { get; }
}
