using System.Collections.Generic;
using UnityEngine;

public class MeshCardGenerator
{
    private MeshCardController answerCardPrefab;
    private Transform parent;
    private MeshCardsManager meshCardsManager;
    [SerializeField] private AnimationCurve _sizeRandomCurve;
    
    public MeshCardController GenerateCard(Sprite sprite)
    {
        var randomRot = Random.Range(0, 360);
        var randomPosition = meshCardsManager.GetNewCardPosition();
        var card = GameObject.Instantiate(answerCardPrefab, randomPosition, Quaternion.Euler(0, 0, randomRot), parent);
        
        card.renderer.material.mainTexture = sprite.texture;

        card.gameObject.transform.localScale = 
            new Vector3(
                sprite.rect.width / 500 * 1.25f, 
                sprite.rect.height / 500 * 1.25f,
                1
            );
        
        var physicsShapeCount = sprite.GetPhysicsShapeCount();

        card.collider.pathCount = physicsShapeCount;

        var physicsShape = new List<Vector2>();

        for ( var i = 0; i < physicsShapeCount; i++ )
        {
            physicsShape.Clear();
            sprite.GetPhysicsShape( i, physicsShape );
            var points = physicsShape.ToArray();
            card.collider.SetPath( i, points );
        }
        
        card.rb.gravityScale = Random.Range(-1f, 1f) * 0.3f;
        var sizeRandom = _sizeRandomCurve.Evaluate(Random.Range(0f, 1f));
        card.rb.mass = sizeRandom;
        card.transform.localScale = Vector3.one * sizeRandom;
        card.transform.position = new Vector3(card.transform.position.x, card.transform.position.y, sizeRandom/4);
        
        return card;
    }
    
}