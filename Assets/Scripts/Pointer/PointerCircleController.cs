using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using R3;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Pointer = System.Reflection.Pointer;

[RequireComponent(typeof(SpriteRenderer))]
public class PointerCircleController : MonoBehaviour
{
    [SerializeField]
    private InputActionProperty _clickAction;
    [SerializeField]
    private InputActionProperty _moveAction;
    [SerializeField]
    private InputActionProperty _touchAction;
    
    private SpriteRenderer _spriteRenderer;
    private bool isPointerMove = false;
    
    private void Start()
    {
        _touchAction.action.started += ctx =>
        {
            if(Touchscreen.current == null) return;
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 0));
            transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
            
            OnClick().Forget();
        };
        _touchAction.action.Enable();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _clickAction.action.started += ctx =>
        {
            OnClick().Forget();
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
    private async UniTask OnClick()
    {
        clickMotion.Complete();

        _spriteRenderer.enabled = true;
        
        // なんかもうちょっとおしゃれにできそうではある。
        var startScale = this.transform.localScale;
        await LMotion.Create(startScale, startScale*0.5f, 0.1f).BindToLocalScale(this.transform).AddTo(clickMotion);
        await LMotion.Create(startScale*0.5f, startScale*1.2f, 0.1f).BindToLocalScale(this.transform).AddTo(clickMotion);
        await LMotion.Create(startScale*1.2f, startScale, 0.05f).BindToLocalScale(this.transform).AddTo(clickMotion);
        
        _spriteRenderer.enabled = false;
    }
    
    private void OnDestroy()
    {
        _clickAction.action.Disable();
        _moveAction.action.Disable();
        _touchAction.action.Disable();
    }
}
