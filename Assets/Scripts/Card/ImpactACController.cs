using Cysharp.Threading.Tasks;
using UnityEngine;

public class ImpactACController : AnswerCardController
{
    protected override void Awake()
    {
        base.Awake();
        LateAddForce().Forget();
    }

    private async UniTask LateAddForce()
    {
        await UniTask.WaitForSeconds(Random.Range(0, 1.5f));
        var force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        _rb.AddForce(force * 2000);
    }
    
}
