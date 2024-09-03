using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class ImpactMCController : MeshCardController
{
    private void Awake()
    {
        LateAddForce().Forget();
    }

    private async UniTask LateAddForce()
    {
        await UniTask.WaitForSeconds(Random.Range(0, 1.3f));
        var force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rb.AddForce(force * 2000);
    }
}
