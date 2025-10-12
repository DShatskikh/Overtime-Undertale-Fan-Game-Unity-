using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Bootstrap : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log(SaveSystem.HasKey("PlayerName"));
        
        if (!SaveSystem.HasKey("PlayerName"))
        {
            SceneManager.LoadScene(1);
            return;
        }

        if (!SaveSystem.HasKey("IsHaveSavingUsableSaveStar"))
        {
            SceneManager.LoadScene(2);
            return;
        }

        SceneManager.LoadScene(9);
    }
}