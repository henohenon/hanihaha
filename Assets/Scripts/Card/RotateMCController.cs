using UnityEngine;

public class RotateMCController : MeshCardController
{
    private readonly float _rotSpeed;
    
    private void Update()
    {
        rb.angularVelocity = _rotSpeed;
    }
}
