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
    private HashSet<AnswerCardController> _currentHoveres = new ();
    private AnswerCardController _currentSelected;
    
    [SerializeField]
    private InputActionProperty _inputActionMap;

    public bool IsCanSelect = false;
    
    private void Start()
    {
        _inputActionMap.action.performed += ctx =>
        {
            if(!IsCanSelect) return;
            if (_currentSelected != null)
            {
                _currentSelected.onClick.OnNext(Unit.Default);
            }
        };
        _inputActionMap.action.Enable();
    }

    private void Update()
    {
        if (_currentHoveres.Count == 0)
        {
            if (_currentSelected != null)
            {
                _currentSelected.onHover.OnNext(false);
            }
            _currentSelected = null;
            return;
        }

        var _distance = float.MaxValue;
        AnswerCardController nearest = null;
        foreach (var hover in _currentHoveres)
        {
            var distance = Vector2.Distance(transform.position, hover.gameObject.transform.position);
            if (distance < _distance)
            {
                _distance = distance;
                nearest = hover;
            }
        }

        if (nearest != _currentSelected)
        {
            if (_currentSelected != null)
            {
                _currentSelected.onHover.OnNext(false);
            }

            _currentSelected = nearest;
            _currentSelected.onHover.OnNext(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var selectable = other.GetComponent<AnswerCardController>();
        if (selectable != null)
        {
            _currentHoveres.Add(selectable);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        var selectable = other.GetComponent<AnswerCardController>();
        if (selectable != null)
        {
            _currentHoveres.Remove(selectable);
        }
    }
    
    private void OnDestroy()
    {
        _inputActionMap.action.Disable();
    }
}
