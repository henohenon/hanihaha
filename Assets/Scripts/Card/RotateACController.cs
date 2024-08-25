using UnityEngine;

public class RotateACController : AnswerCardController
{
    [SerializeField]
    private AnimationCurve _rotateCurve;
    
    private float _rotSpeed;
    
    protected override void Awake()
    {
        base.Awake();
        _rotSpeed = 
            _rotateCurve.Evaluate(Random.Range(0f, 1f)) * 
            (Random.value > 0.5f ? 1 : -1)
        * 15;
    }
    
    private void Update()
    {
        _rb.angularVelocity = _rotSpeed;
    }
}
