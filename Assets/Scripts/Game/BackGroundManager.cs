using System;
using Alchemy.Inspector;
using LitMotion;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    [SerializeField]
    private Transform _circleTrans;
    [AssetsOnly]
    [SerializeField] private Material _colorMaterial;
    [AssetsOnly]
    [SerializeField] private Material _whiteMaterial;


    private MotionHandle rotHandle;
    private MotionHandle colorHandle;
    
    private void Awake()
    {
        CreateRotMotion();
        colorHandle = LMotion.Create(0f, 1f, 120f).WithLoops(-1, LoopType.Restart).BindWithState(_colorMaterial,
            (x, _mat) =>
            {
                if(colorHandle.PlaybackSpeed == 0) return;
                var color = Color.HSVToRGB(x, 1, 1);
                _mat.color = color;
            });
        _whiteMaterial.color = Color.white;
        ChangeScreen(ScreenType.Title);
    }

    private void CreateRotMotion(bool _isClockwise = true)
    {
        var nowX = _circleTrans.localEulerAngles.x;
        var addRot = _isClockwise ? 360 : -360;
        rotHandle = LMotion.Create(nowX, nowX+addRot, 60f).WithLoops(-1, LoopType.Restart).BindWithState(_circleTrans,
            (x, _trans) =>
            {
                _trans.localEulerAngles = new Vector3(x, 0, 0);
            });
    }


    private bool _clockwise = true;
    public void ChangeScreen(ScreenType type)
    {
        switch (type)
        {
            case ScreenType.Title:
                rotHandle.PlaybackSpeed = 0;
                colorHandle.PlaybackSpeed = 0f;
                // 透明にしておく
                _colorMaterial.color = new Color(0, 0, 0, 0);
                break;
            case ScreenType.NextTarget:
                break;
            case ScreenType.Game:
                colorHandle.PlaybackSpeed = 1;
                
                
                if (rotHandle.IsActive())
                {
                    rotHandle.Cancel();
                }
                _clockwise = !_clockwise;
                CreateRotMotion(_clockwise);
                break;
            case ScreenType.GameOver:
                rotHandle.PlaybackSpeed = 0;
                colorHandle.PlaybackSpeed = 0;
                _colorMaterial.color = Color.black;
                break;
            case ScreenType.HighScore:
                rotHandle.PlaybackSpeed = 0;
                colorHandle.PlaybackSpeed = 100;
                _colorMaterial.color = Color.green;
                break;
        }
    }
    
    private bool _oldIsLimit = false;
    public void SetLimit(bool isLimit)
    {
        if (_oldIsLimit == isLimit) return;
        _oldIsLimit = isLimit;
        
        if (isLimit){
            //_whiteMaterial.color = Color.black;
            rotHandle.PlaybackSpeed = 30;
        }
        else
        {
            _whiteMaterial.color = Color.white;
            rotHandle.PlaybackSpeed = 1;
        }
    }
}
