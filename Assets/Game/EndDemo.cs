using UnityEngine;

public sealed class EndDemo : MonoBehaviour
{
    private void Start()
    {
        MusicPlayer.Instance.Stop();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Application.Quit();
        }
    }
}