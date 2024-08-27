using UnityEngine;

public class ReflectionACController : ImpactACController
{
    [SerializeField]
    private AnimationCurve _reflectionCurve;
    [SerializeField]
    private LayerMask _borderLayer;
    private Vector2 _moveSpeed;
    protected override void Awake()
    {
        base.Awake();
        _moveSpeed = new Vector2(
            _reflectionCurve.Evaluate(Random.Range(0f, 1f)) * (Random.value > 0.5f ? 1 : -1),
            _reflectionCurve.Evaluate(Random.Range(0f, 1f)) * (Random.value > 0.5f ? 1 : -1)
        ) * 15;    }
    
    public void Update()
    {
        _rb.linearVelocity = _moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (IsSameLayer(other.gameObject, _borderLayer)) {
            _moveSpeed = Vector2.Reflect(_moveSpeed, other.GetContact(0).normal);
        }
    }
}
