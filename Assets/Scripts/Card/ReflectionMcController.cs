using UnityEngine;

public class ReflectionMcController : MeshCardController
{
    [SerializeField]
    private AnimationCurve reflectionCurve;
    [SerializeField]
    private LayerMask borderLayer;
    
    private Vector2 _moveSpeed;
    private void Awake()
    {
        _moveSpeed = new Vector2(
            reflectionCurve.Evaluate(Random.Range(0f, 1f)) * (Random.value > 0.5f ? 1 : -1),
            reflectionCurve.Evaluate(Random.Range(0f, 1f)) * (Random.value > 0.5f ? 1 : -1)
        ) * 15;    }
    
    private void Update()
    {
        rb.linearVelocity = _moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (IsSameLayer(other.gameObject, borderLayer)) {
            _moveSpeed = Vector2.Reflect(_moveSpeed, other.GetContact(0).normal);
        }
    }
}
