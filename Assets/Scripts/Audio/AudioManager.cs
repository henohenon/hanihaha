using Alchemy.Inspector;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _effectSource;
    [SerializeField] 
    private AudioSource _highPitchSource;
    [SerializeField]
    private AudioSource _limitSource;

    
    [SerializeField]
    private AudioClip _successClip;
    [SerializeField]
    private AudioClip _failClip;
    [SerializeField]
    private AudioClip _nextTargetClip;
    [SerializeField]
    private AudioClip _gameStartClip;
    [SerializeField]
    private AudioClip _gameOverClip;
    [SerializeField]
    private AudioClip _highScoreClip;
    
    public void PlayGameStartSound()
    {
        _effectSource.PlayOneShot(_gameStartClip);
    }
    
    [Button]
    public void PlaySuccessSound(bool isCombo)
    {
        if (isCombo)
        {
            _highPitchSource.PlayOneShot(_successClip);
        }
        else
        {
            _effectSource.PlayOneShot(_successClip);
        }
    }
    
    [Button]
    public void PlayFailSound()
    {
        _effectSource.PlayOneShot(_failClip);
    }
    
    [Button]
    public void PlayNextTargetSound()
    {
        _effectSource.PlayOneShot(_nextTargetClip);
    }
    
    public void PlayGameOverSound()
    {
        _effectSource.PlayOneShot(_gameOverClip);
    }
    
    public void PlayHighScoreSound()
    {
        _effectSource.PlayOneShot(_highScoreClip);
    }
    
    public void SetIsPlayLimit(bool isPlay)
    {
        if(_limitSource.isPlaying == isPlay) return;
        if (isPlay)
        {
            _limitSource.Play();
        }
        else
        {
            _limitSource.Stop();
        }
    }
}
