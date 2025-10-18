using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class EndBattleAnimation : MonoBehaviour
{
    private IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);
        var blackScreen = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y);
        
        var delta = 0f;
        
        while (delta < 1)
        {
            blackScreen.color = new Color(0, 0, 0, delta);
            delta += Time.deltaTime * 2;
            yield return null;
        }
        
        Destroy(Soul.Instance.gameObject);
        yield return SceneManager.UnloadSceneAsync(11);
        
        if (Player.Instance)
            Player.Instance.enabled = true;
        
        FindAnyObjectByType<Camera>(FindObjectsInactive.Include).gameObject.SetActive(true);
        Destroy(gameObject);
    }
}