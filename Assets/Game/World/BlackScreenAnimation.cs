using System.Collections;
using UnityEngine;

public sealed class BlackScreenAnimation : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _blackScreen;
    
    public IEnumerator AwaitShowAnimation()
    {
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y);
        Player.Instance.enabled = false;

        var delta = 0f;
        
        while (delta < 1f)
        {
            delta += Time.deltaTime * 2;
            _blackScreen.color = new Color(0, 0, 0, delta);
            yield return null;
        }
    }
    
    public IEnumerator AwaitHideAnimation()
    {
        var delta = 1f;

        while (delta > 0f)
        {
            delta -= Time.deltaTime * 2;
            _blackScreen.color = new Color(0, 0, 0, delta);
            yield return null;
        }

        Player.Instance.enabled = true;
    }
}