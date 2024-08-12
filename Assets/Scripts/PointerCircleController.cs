using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerCircleController : MonoBehaviour
{
    private void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
    }
}
