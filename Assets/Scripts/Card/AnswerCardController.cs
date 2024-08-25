using System.Collections.Generic;
using R3;
using UnityEngine;

public class AnswerCardController : IPointable
{
    public Subject<bool> onHover { get; } = new ();
    public Subject<Unit> onClick { get; } = new ();
    
    private bool _isAnswered = false;
    private GameObject _obj;
    

    public AnswerCardController(Sprite sprite, GameObject obj)
    {
        _obj = obj;
        var _collider = _obj.GetComponent<PolygonCollider2D>();
        var _meshRenderer = _obj.GetComponent<MeshRenderer>();
        
        var _spriteRenderer = _obj.GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = sprite;
        _meshRenderer.material.mainTexture = sprite.texture;

        _obj.transform.localScale = 
            new Vector3(
                sprite.rect.size.y / 500, 
                sprite.rect.size.x / 500,
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

    public void Destroy()
    {
        GameObject.Destroy(_obj);
    }
}

public interface IPointable
{
    public Subject<bool> onHover { get; }
    public Subject<Unit> onClick { get; }
}
