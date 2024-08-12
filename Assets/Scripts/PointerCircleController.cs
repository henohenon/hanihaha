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
    private InputAction _clickAction;
    [SerializeField]
    private InputAction _moveAction;
    
    private void Start()
    {
        _clickAction.performed += ctx =>
        {
            OnClick();
        };
        _clickAction.Enable();
        
        // マウス位置に移動
        _moveAction.performed += ctx =>
        {
            Vector2 pos = ctx.ReadValue<Vector2>();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 0));
        
            transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
        };
        _moveAction.Enable();
    }
    
    // クリック時アニメーション
    private CompositeMotionHandle clickMotion = new CompositeMotionHandle();
    private async void OnClick()
    {
        clickMotion.Complete();
        // なんかもうちょっとおしゃれにできそうではある。
        float startScale = this.transform.localScale.x;
        await LMotion.Create(GetOneValueVector3(startScale), GetOneValueVector3(startScale*0.5f), 0.1f).BindToLocalScale(this.transform).AddTo(clickMotion);
        await LMotion.Create(GetOneValueVector3(startScale*0.5f), GetOneValueVector3(startScale*1.1f), 0.04f).BindToLocalScale(this.transform).AddTo(clickMotion);
        await LMotion.Create(GetOneValueVector3(startScale*1.1f), GetOneValueVector3(startScale), 0.015f).BindToLocalScale(this.transform).AddTo(clickMotion);
    }
    
    private Vector3 GetOneValueVector3(float value)
    {
        return new Vector3(value, value, value);
    }
    
    private void OnDestroy()
    {
        _clickAction.Disable();
        _moveAction.Disable();
    }
}
