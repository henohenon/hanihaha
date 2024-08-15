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
    private AudioClip _startGameClip;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    [Button]
    public void PlaySuccessEffect()
    {
        _audioSource.PlayOneShot(_successClip);
    }
    
    [Button]
    public void PlayFailEffect()
    {
        _audioSource.PlayOneShot(_failClip);
    }
    
    [Button]
    public void PlayStarGameEffect()
    {
        _audioSource.PlayOneShot(_startGameClip);
    }
    
    public void SetIsPlayLimit(bool isPlay)
    {
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
