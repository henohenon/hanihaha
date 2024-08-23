using LitMotion;
using R3;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField]
    private AudioManager _audioManager;
    [SerializeField]
    private GameUIManager _gameUIManager;

    
    private float _timer = 10;
    private MotionHandle _timerHandle;
    
    private Subject<Unit> _onTimeUp = new Subject<Unit>();
    public Observable<Unit> OnTimeUp => _onTimeUp;

    
    private void ReStartTimer()
    {
        if (_timerHandle.IsActive())
        {
            _timerHandle.Cancel();
        }
        
        
        var isLimit = _timer <= 5;
        _audioManager.SetIsPlayLimit(isLimit);
        _gameUIManager.SetLimit(isLimit);
        
        _timerHandle = LMotion.Create(_timer, 0, _timer).WithOnComplete(() =>
        {
            _onTimeUp.OnNext(Unit.Default);
        }).BindWithState(_gameUIManager, (_time, _ui) =>
        {
            _timer = _time;
            _ui.SetTimer(_timer);
            var isLimit = _timer <= 5;
            _audioManager.SetIsPlayLimit(isLimit);
            _ui.SetLimit(isLimit);
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
        _gameUIManager.ShowPlusTime(addTime);
    }
    
    public void MinusTime(float minusTime)
    {
        _timer -= minusTime;
        ReStartTimer();
        _gameUIManager.ShowMinusTime(minusTime);
    }
    
    public void SetPause(bool isPause)
    {
        _timerHandle.PlaybackSpeed = isPause ? 0 : 1;
    }
}
