using System.Collections;
using UnityEngine;

public sealed class QuitingScreen : MonoBehaviour
{
    [SerializeField]
    private TextMesh _label;

    private Coroutine _coroutine;
    
    private void Update()
    {
        if (Camera.main)
        {
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _coroutine = StartCoroutine(AwaitQuitting());
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (_coroutine != null)
            {
                _label.gameObject.SetActive(false);
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }
    }

    private IEnumerator AwaitQuitting()
    {
        _label.gameObject.SetActive(true);
        _label.text = "Quitting";
        var alpha = 0f;
        
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            _label.color = new Color(0, 1, 0, alpha);
            yield return null;
        }

        _label.text += '.';
        yield return new WaitForSeconds(0.5f);
        _label.text += '.';
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
}