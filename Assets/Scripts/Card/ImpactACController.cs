using UnityEngine;

public class ImpactACController : AnswerCardController
{
    protected override void Awake()
    {
        base.Awake();
        var force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        _rb.AddForce(force * 2000);
    }
    
}
