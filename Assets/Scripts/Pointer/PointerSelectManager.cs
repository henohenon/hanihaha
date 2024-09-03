using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Pointer = System.Reflection.Pointer;

public class PointerSelectManager : MonoBehaviour
{
    private HashSet<MeshCardController> _currentHoveres = new ();
    private MeshCardController _currentSelected;
    
    [SerializeField]
    private InputActionProperty _mouseClick;
    [SerializeField]
    private InputActionProperty _touchClick;


    public bool IsCanSelect = false;
    
    private void Start()
    {
        _mouseClick.action.started += ctx =>
        {
            if(Mouse.current == null) return;
            if(!IsCanSelect) return;
            if (_currentSelected != null)
            {
                _currentSelected.onClick.OnNext(Unit.Default);
            }
        };
        _mouseClick.action.Enable();
        _touchClick.action.started += ctx =>
        {
            if(Touchscreen.current == null) return;
            if(!IsCanSelect) return;
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 0));
            var hits = Physics2D.OverlapCircleAll(worldPosition, this.transform.localScale.x / 2);
            if(hits.Length == 0) return;
            var minDistance = float.MaxValue;
            MeshCardController nearest = null;
            foreach (var hit in hits)
            {
                var selectable = hit.GetComponent<MeshCardController>();
                if (selectable != null)
                {
                    var distance = Vector2.Distance(worldPosition, selectable.gameObject.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearest = selectable;
                    }
                }
            }
            if (nearest != null)
            {
                nearest.onClick.OnNext(Unit.Default);
            }
        };
        _touchClick.action.Enable();
    }

    private void Update()
    {
        if(Mouse.current == null) return;
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
        MeshCardController nearest = null;
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
        var selectable = other.GetComponent<MeshCardController>();
        if (selectable != null)
        {
            _currentHoveres.Add(selectable);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        var selectable = other.GetComponent<MeshCardController>();
        if (selectable != null)
        {
            _currentHoveres.Remove(selectable);
        }
    }
    
    private void OnDestroy()
    {
        _mouseClick.action.Disable();
        _touchClick.action.Disable();
    }
}
