using UnityEngine;

public sealed class PlayMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip _music;

    private void Start()
    {
        MusicPlayer.Instance.Play(_music);
    }
}