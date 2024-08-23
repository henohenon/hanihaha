using System;
using System.Collections;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using R3;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointerCircleController : MonoBehaviour
{
    [SerializeField]
    private InputActionProperty _clickAction;
    [SerializeField]
    private InputActionProperty _moveAction;
    
    private SpriteRenderer _spriteRenderer;
    private bool isPointerMove = false;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _clickAction.action.performed += ctx =>
        {
            OnClick();
        };
        _clickAction.action.Enable();
        
        // マウス位置に移動
        _moveAction.action.performed += ctx =>
        {
            isPointerMove = true;
            _spriteRenderer.enabled = true;
            Vector2 pos = ctx.ReadValue<Vector2>();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 0));
        
            transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
        };
        _moveAction.action.Enable();
    }
    
    // クリック時アニメーション
    private CompositeMotionHandle clickMotion = new CompositeMotionHandle();
    private async void OnClick()
    {
        clickMotion.Complete();

        _spriteRenderer.enabled = true;

        // デバイスを取得
        var pointer = Pointer.current;
        if (pointer != null)
        {
            // クリック位置を取得
            var pos = pointer.position.ReadValue();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 0));
            
            transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
        }
        
        // なんかもうちょっとおしゃれにできそうではある。
        float startScale = this.transform.localScale.x;
        await LMotion.Create(GetOneValueVector3(startScale), GetOneValueVector3(startScale*0.5f), 0.1f).BindToLocalScale(this.transform).AddTo(clickMotion);
        await LMotion.Create(GetOneValueVector3(startScale*0.5f), GetOneValueVector3(startScale*1.1f), 0.04f).BindToLocalScale(this.transform).AddTo(clickMotion);
        await LMotion.Create(GetOneValueVector3(startScale*1.1f), GetOneValueVector3(startScale), 0.015f).BindToLocalScale(this.transform).AddTo(clickMotion);
        if (!isPointerMove)
        {
            _spriteRenderer.enabled = false;
        }
    }
    
    private Vector3 GetOneValueVector3(float value)
    {
        return new Vector3(value, value, value);
    }
    
    private void OnDestroy()
    {
        _clickAction.action.Disable();
        _moveAction.action.Disable();
    }
}
