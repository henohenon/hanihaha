using System;
using System.Collections;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using R3;
using UnityEditor;
using UnityEngine;

public class PointerCircleController : MonoBehaviour
{
   
    private void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
        
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Down");
            OnClick();
        }
    }

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
}
