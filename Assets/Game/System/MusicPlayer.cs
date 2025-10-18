using UnityEngine;

public sealed class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;
    
    private static MusicPlayer _instance;
    public static MusicPlayer Instance => _instance;
    public AudioClip GetClip => _audioSource.clip;

    private void Start()
    {
        _instance = this;
    }

    public void Play(AudioClip clip)
    {
        if (_audioSource.clip == clip)
            return;
        
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void Stop()
    {
        _audioSource.Stop();
    }
}