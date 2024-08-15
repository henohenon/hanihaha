using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PointerSelectManager : MonoBehaviour
{
    private HashSet<IPointable> _currentSelectable;
    
    [SerializeField]
    private InputActionProperty _inputActionMap;
    
    private void Start()
    {
        _currentSelectable = new HashSet<IPointable>();
        
        _inputActionMap.action.performed += ctx =>
        {
            if (_currentSelectable != null)
            {
                foreach (var selectable in _currentSelectable)
                {
                    if(selectable == null) continue;
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
