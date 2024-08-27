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
        var sizeRandom = _sizeRandomCurve.Evaluate(Random.Range(0f, 1f));
        _rb.mass = sizeRandom;
        transform.localScale = Vector3.one * sizeRandom;
        transform.position = new Vector3(transform.position.x, transform.position.y, -sizeRandom/10);
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
        
        onClick.Subscribe(_ =>
        {
            //_spriteRenderer.enabled = false;
            _collider.enabled = false;
        }).AddTo(this);
    }

    protected bool IsSameLayer(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) != 0;
    }

    public void ChangeColor(Color color)
    {
        _renderer.material.color = color;
    }
}


public interface IPointable
{
    public Subject<bool> onHover { get; }
    public Subject<Unit> onClick { get; }
}
