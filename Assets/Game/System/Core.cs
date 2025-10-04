using UnityEngine;

public sealed class Core : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsByType<Core>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }
}