using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class MeshCardController : MonoBehaviour, IPointable
{
    public Subject<bool> onHover { get; } = new ();
    public Subject<Unit> onClick { get; } = new ();
    
    public PolygonCollider2D collider;
    public MeshRenderer renderer;
    public Rigidbody2D rb;
    
    public void ChangeColor(Color color)
    {
        renderer.material.color = color;
    }
    
    protected bool IsSameLayer(GameObject obj, LayerMask layer)
    {
        return obj.layer == layer;
    }
}


public interface IPointable
{
    public Subject<bool> onHover { get; }
    public Subject<Unit> onClick { get; }
}
