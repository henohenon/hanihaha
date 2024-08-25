using System;
using Alchemy.Inspector;
using LitMotion;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField]
    private Transform _circleTrans;
    [AssetsOnly]
    [SerializeField] private Material _colorMaterial;
    [AssetsOnly]
    [SerializeField] private Material _whiteMaterial;


    private MotionHandle circleRotHandle;
    private MotionHandle lightRotHandle;
    private MotionHandle colorHandle;
    
    private void Awake()
    {
        CreateLightMotion();
        CreateRotMotion();
        colorHandle = LMotion.Create(0f, 1f, 120f).WithLoops(-1, LoopType.Restart).BindWithState(_colorMaterial,
            (x, _mat) =>
            {
                if(colorHandle.PlaybackSpeed == 0) return;
                var color = Color.HSVToRGB(x, 0.65f, 1);
                _mat.color = color;
            });
        //_whiteMaterial.color = Color.white;
        ChangeScreen(ScreenType.Title);
    }

    private void CreateRotMotion(bool _isClockwise = true)
    {
        var nowX = _circleTrans.localEulerAngles.x;
        var addRot = _isClockwise ? 360 : -360;
        circleRotHandle = LMotion.Create(nowX, nowX+addRot, 60f).WithLoops(-1, LoopType.Restart).BindWithState(_circleTrans,
            (x, _trans) =>
            {
                _trans.localEulerAngles = new Vector3(x, 0, 0);
            });
    }

    private void CreateLightMotion()
    {
        lightRotHandle = LMotion.Create(0, 360f, 600f).WithLoops(-1, LoopType.Restart).BindWithState(_light.transform,
            (x, _trans) =>
            {
                if(lightRotHandle.PlaybackSpeed == 0) return;
                var _nomRX = Math.Abs((x - 180) / 90);
                var _nomRY = Math.Abs(((x + 90) % 360 - 180) / 90);
                _trans.localEulerAngles = new Vector3(_nomRX-1, _nomRY-1, 0).normalized*15;
            });
    }


    private bool _clockwise = true;
    public void ChangeScreen(ScreenType type)
    {
        switch (type)
        {
            case ScreenType.Title:
                _light.intensity = 0.4f;
                circleRotHandle.PlaybackSpeed = 0;
                colorHandle.PlaybackSpeed = 0f;
                // 透明にしておく
                _colorMaterial.color = new Color(0, 0, 0, 0);
                lightRotHandle.PlaybackSpeed = 0;
                _light.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case ScreenType.NextTarget:
                _light.intensity = 0.7f;
                lightRotHandle.PlaybackSpeed = 1;
                break;
            case ScreenType.Game:
                _light.intensity = 0.7f;
                colorHandle.PlaybackSpeed = 1;
                lightRotHandle.PlaybackSpeed = 1;
                
                if (circleRotHandle.IsActive())
                {
                    circleRotHandle.Cancel();
                }
                _clockwise = !_clockwise;
                CreateRotMotion(_clockwise);
                break;
            case ScreenType.GameOver:
                _light.intensity = 0f;
                lightRotHandle.PlaybackSpeed = 0;
                _light.transform.rotation = Quaternion.Euler(0, 0, 0);
                circleRotHandle.PlaybackSpeed = 0;
                colorHandle.PlaybackSpeed = 0;
                _colorMaterial.color = Color.black;
                break;
            case ScreenType.HighScore:
                _light.intensity = 3f;
                lightRotHandle.PlaybackSpeed = 600;
                circleRotHandle.PlaybackSpeed = 0;
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
            lightRotHandle.PlaybackSpeed = 3;
            circleRotHandle.PlaybackSpeed = 30;
        }
        else
        {
            //_whiteMaterial.color = Color.white;
            lightRotHandle.PlaybackSpeed = 1;
            circleRotHandle.PlaybackSpeed = 1;
        }
    }
}
