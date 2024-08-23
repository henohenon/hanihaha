using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;

public class ComboCardController
{
    private TextMesh _textMesh;
    public ComboCardController(int comboIndex, TextMesh _textMesh)
    {
        this._textMesh = _textMesh;
        switch (comboIndex)
        {
            case 0:
                GameObject.Destroy(_textMesh.gameObject);
                return;
            case 1:
                _textMesh.text = $"{comboIndex} Combo";
                break;
            case 2:
                _textMesh.text = $"{comboIndex} Combo";
                break;
            case 3:
                _textMesh.text = $"{comboIndex} Combo!";
                _textMesh.color = Color.green;
                break;
            case 4:
                _textMesh.text = $"{comboIndex} Combo!";
                _textMesh.color = Color.green;
                break;
            case 5:
                _textMesh.text = $"{comboIndex} Combo!!";
                _textMesh.color = Color.yellow;
                break;
            case 6:
                _textMesh.text = $"{comboIndex} Combo!!";
                _textMesh.color = Color.yellow;
                break;
            case 7:
                _textMesh.text = $"{comboIndex} Combo!!!";
                _textMesh.color = Color.red;
                break;
            case 8:
                _textMesh.text = $"{comboIndex} Combo!!!";
                _textMesh.color = Color.red;
                break;
            case 9:
                _textMesh.text = $"{comboIndex} Combo!!!";
                _textMesh.color = Color.red;
                break;
            default:
                _textMesh.text = $"{comboIndex} Combo!!!!!";
                LMotion.Create(0f, 1f, 1f).WithLoops(-1, LoopType.Restart).BindWithState(_textMesh, (x, _mesh) =>
                {
                    Debug.Log(x);
                    _mesh.color = Color.HSVToRGB(x, 1, 1);
                }).AddTo(_textMesh);
                break;
        }
        
        AutoDestroy();
    }

    private async void AutoDestroy()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
        GameObject.Destroy(_textMesh.gameObject);
    }
}
