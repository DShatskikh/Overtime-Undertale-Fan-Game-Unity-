using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Bootstrap : MonoBehaviour
{
    private void Awake()
    {
        //SceneManager.LoadScene(1);
        SceneManager.LoadScene(5);
    }
}