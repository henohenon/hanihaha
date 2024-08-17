using Alchemy.Inspector;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _effectSource;
    [SerializeField]
    private AudioSource _limitSource;
    
    [SerializeField]
    private AudioClip _successClip;
    [SerializeField]
    private AudioClip _failClip;
    [SerializeField]
    private AudioClip _nextTargetClip;
    [SerializeField]
    private AudioClip _gameOverClip;
    [SerializeField]
    private AudioClip _highScoreClip;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    [Button]
    public void PlaySuccessSound()
    {
        _audioSource.PlayOneShot(_successClip);
    }
    
    [Button]
    public void PlayFailSound()
    {
        _audioSource.PlayOneShot(_failClip);
    }
    
    [Button]
    public void PlayNextTargetSound()
    {
        _audioSource.PlayOneShot(_nextTargetClip);
    }
    
    public void PlayGameOverSound()
    {
        _audioSource.PlayOneShot(_gameOverClip);
    }
    
    public void PlayHighScoreSound()
    {
        _audioSource.PlayOneShot(_highScoreClip);
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
