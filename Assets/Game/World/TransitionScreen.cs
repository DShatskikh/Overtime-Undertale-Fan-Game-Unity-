using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class TransitionScreen : MonoBehaviour
{
    public static void Transition(int indexScene, int pointIndex = -1)
    {
        var prefab = Resources.Load<TransitionScreen>("Transition Screen");
        var instance = Instantiate(prefab);
        DontDestroyOnLoad(instance);
        instance.StartCoroutine(instance.AwaitTransition(indexScene, pointIndex));
    }

    private IEnumerator AwaitTransition(int indexScene, int pointIndex = -1)
    {
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y);
        Player.Instance.enabled = false;
        var direction = Player.Instance.GetDirection;
        var spriteRenderer = GetComponent<SpriteRenderer>();

        var delta = 0f;
        
        while (delta < 1f)
        {
            delta += Time.deltaTime;
            spriteRenderer.color = new Color(0, 0, 0, delta);
            yield return null;
        }

        yield return SceneManager.LoadSceneAsync(indexScene);

        if (!Player.Instance)
        {
            Destroy(gameObject);
            yield break;
        }
        
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y);
        Player.Instance.enabled = false;

        if (pointIndex != -1)
        {
            foreach (var transitionPoint in FindObjectsByType<TransitionPoint>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (transitionPoint.GetIndex == pointIndex)
                {
                    Player.Instance.transform.position = transitionPoint.transform.position;
                    break;
                }
            }
        }
        
        Player.Instance.SetDirection(direction);
        
        delta = 1f;
        
        while (delta > 0f)
        {
            delta -= Time.deltaTime;
            spriteRenderer.color = new Color(0, 0, 0, delta);
            yield return null;
        }

        Destroy(gameObject);
        Player.Instance.enabled = true;
    }
}