using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class TimerUIManager : IDisposable
{
    private Label _timerLabel;
    private TextElement _minusTime;
    private TextElement _plusTime;
    
    public TimerUIManager(VisualElement root)
    {
        _timerLabel = root.Q<Label>("Time");
        _minusTime = root.Q<TextElement>("MinusTime");
        _plusTime = root.Q<TextElement>("PlusTime");
    }
    // タイマーを設定
    public void SetTimer(float time)
    {
        _timerLabel.text = time.ToString("F2");
    }

    // プラスのタイマーを表示
    private CancellationTokenSource _plusTimeCts;
    public void ShowPlusTime(float time)
    {
        ShowTimer(time, true, _plusTime, _plusTimeCts);
    }

    // マイナスのタイマーを表示
    private CancellationTokenSource _minusTimeCts;
    public void ShowMinusTime(float time)
    {
        ShowTimer(time, false, _minusTime, _minusTimeCts);
    }

    // キャンセル可能なタイマー表示
    private async void ShowTimer(float time, bool isPlus, TextElement elem, CancellationTokenSource _cts)
    {
        
        // 既存のタスクがあればキャンセル
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        try
        {
            var timeString = time.ToString("F2");
            elem.text = isPlus ? $"+{timeString}" : $"-{timeString}";
            elem.style.display = DisplayStyle.Flex;

            // 一定時間待機するが、キャンセルが可能
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: _cts.Token);

            elem.style.display = DisplayStyle.None;
        }
        catch (OperationCanceledException)
        {
            // タスクがキャンセルされたとき。tryは必ずcatchがいるのです。
        }
        finally
        {
            _cts.Dispose();
        }
    }
    
    public void Dispose()
    {
        _plusTimeCts?.Dispose();
        _minusTimeCts?.Dispose();
    }
}


public interface IDisposable
{
    public void Dispose();
}