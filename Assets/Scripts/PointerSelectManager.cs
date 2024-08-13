using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PointerSelectManager : MonoBehaviour
{
    private HashSet<ISelectable> _currentSelectable;
    
    [SerializeField]
    private InputActionProperty _inputActionMap;
    
    private void Start()
    {
        _currentSelectable = new HashSet<ISelectable>();
        
        _inputActionMap.action.performed += ctx =>
        {
            if (_currentSelectable != null)
            {
                foreach (var selectable in _currentSelectable)
                {
                    selectable.OnSelect();
                }                
            }
        };
        _inputActionMap.action.Enable();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var selectable = other.GetComponent<ISelectable>();
        if (selectable != null)
        {
            _currentSelectable.Add(selectable);
            selectable.OnHover();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        var selectable = other.GetComponent<ISelectable>();
        if (selectable != null)
        {
            selectable.OnUnhover();
            _currentSelectable.Remove(selectable);
        }
    }
    
    private void OnDestroy()
    {
        _inputActionMap.action.Disable();
    }
}
