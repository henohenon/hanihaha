using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PointerSelectManager : MonoBehaviour
{
    private HashSet<IPointable> _currentSelectable = new HashSet<IPointable>();
    
    [SerializeField]
    private InputActionProperty _inputActionMap;

    public bool IsCanSelect = false;
    
    private void Start()
    {
        _inputActionMap.action.performed += ctx =>
        {
            if(!IsCanSelect) return;
            var hits = Physics2D.CircleCastAll(transform.position, transform.localScale.x/2, Vector2.zero);
            
            foreach (var hit in hits)
            {
                var selectable = hit.collider.GetComponent<IPointable>();
                if (selectable != null)
                {
                    selectable.onClick.OnNext(Unit.Default);
                }
            }
        };
        _inputActionMap.action.Enable();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var selectable = other.GetComponent<IPointable>();
        if (selectable != null)
        {
            selectable.onHover.OnNext(true);
            _currentSelectable.Add(selectable);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        var selectable = other.GetComponent<IPointable>();
        if (selectable != null)
        {
            selectable.onHover.OnNext(false);
            _currentSelectable.Remove(selectable);
        }
    }
    
    private void OnDestroy()
    {
        _inputActionMap.action.Disable();
    }
}
