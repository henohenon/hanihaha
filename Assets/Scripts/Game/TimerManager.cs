using LitMotion;
using R3;
using UnityEngine;

public class TimerManager
{
    private ScreenUIManager _screenUIManager;
    private TimerUIManager _timerUIManager;
    
    private float _timer = 10;
    private MotionHandle _timerHandle;
    
    private Subject<Unit> _onTimeUp = new ();
    public Observable<Unit> OnTimeUp => _onTimeUp;

    private ReactiveProperty<bool> _onLimit = new ();
    public Observable<bool> OnLimit => _onLimit;
    
    public TimerManager(ScreenUIManager screenUIManager, TimerUIManager timerUIManager)
    {
        _screenUIManager = screenUIManager;
        _timerUIManager = timerUIManager;
        _timerUIManager.SetTimer(_timer);
        //ReStartTimer();
        
        
        /*
        _audioManager.SetIsPlayLimit(isLimit);
        _screenUIManager.SetLimit(isLimit);
        _audioManager.SetIsPlayLimit(isLimit);
        _backgroundManager.SetLimit(isLimit);
        _ui.SetLimit(isLimit);
        */
    }
    
    public void ReStartTimer()
    {
        if (_timerHandle.IsActive())
        {
            _timerHandle.Cancel();
        }
        
        _timerHandle = LMotion.Create(_timer, 0, _timer).WithOnComplete(() =>
        {
            _onTimeUp.OnNext(Unit.Default);
        }).BindWithState(_timerUIManager, (_time, _ui) =>
        {
            _timer = _time;
            _ui.SetTimer(_timer);
            _onLimit.Value = _timer <= 5;
        }).AddTo(this);
    }

    public void SetTime(float time)
    {
        _timer = time;
        ReStartTimer();
    }
    public void AddTime(float addTime)
    {
        if(addTime <= 0) return;
        _timer += addTime;
        ReStartTimer();
        _timerUIManager.ShowPlusTime(addTime);
    }
    
    public void MinusTime(float minusTime)
    {
        _timer -= minusTime;
        ReStartTimer();
        _timerUIManager.ShowMinusTime(minusTime);
    }
    
    public void SetPause(bool isPause)
    {
        _timerHandle.PlaybackSpeed = isPause ? 0 : 1;
    }
}
