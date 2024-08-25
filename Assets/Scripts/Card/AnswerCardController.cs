using System.Collections.Generic;
using R3;
using UnityEngine;

public class AnswerCardController : MonoBehaviour, IPointable
{
    public Subject<bool> onHover { get; } = new ();
    public Subject<Unit> onClick { get; } = new ();
    
    private bool _isAnswered = false;
    private GameObject _obj;
    
    [SerializeField]
    private PolygonCollider2D _collider;
    [SerializeField]
    private MeshRenderer _renderer;

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
        }).AddTo(_obj);
        
        onClick.Subscribe(_ =>
        {
            //_spriteRenderer.enabled = false;
            _collider.enabled = false;
        }).AddTo(_obj);
    }
}

public interface IPointable
{
    public Subject<bool> onHover { get; }
    public Subject<Unit> onClick { get; }
}
