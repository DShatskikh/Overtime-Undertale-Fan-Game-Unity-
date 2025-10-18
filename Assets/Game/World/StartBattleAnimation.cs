using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class StartBattleAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject _black;

    private AudioSource _audioSource;
    private Soul _soul;
    private int _sceneIndex;
    private Action _action;
    private Vector2 _targetPosition;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Init(int sceneIndex, Vector2 targetPosition, Action action)
    {
        _sceneIndex = sceneIndex;
        _targetPosition = targetPosition;
        _action = action;
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y);
        StartCoroutine(AwaitAnimation());
    }

    private IEnumerator AwaitAnimation()
    {
        _soul = Instantiate(Resources.Load<Soul>("Soul"));
        _soul.enabled = false;
        _soul.transform.position = Player.Instance.transform.position;

        for (int i = 0; i < 3; i++)
        {
            _soul.gameObject.SetActive(true);
            _black.SetActive(true);
            yield return new WaitForSeconds(0.1f);

            if (i == 0)
            {
                _audioSource.Play();
            }
            
            if (i != 2)
            {
                _soul.gameObject.SetActive(false);
                _black.SetActive(false);
                yield return new WaitForSeconds(0.1f); 
            }
        }

        var speed = Vector3.Distance(_soul.transform.position, transform.position + (Vector3)_targetPosition); // 5
        
        while (Vector3.Distance(_soul.transform.position, transform.position + (Vector3)_targetPosition) >= 0.1f)
        {
            _soul.transform.position = Vector3.MoveTowards(_soul.transform.position, transform.position + (Vector3)_targetPosition, Time.deltaTime * speed);
            yield return null;
        }

        _soul.transform.position = transform.position + (Vector3)_targetPosition;
        
        var camera = Camera.main;
        yield return SceneManager.LoadSceneAsync(_sceneIndex, LoadSceneMode.Additive);
        camera.gameObject.SetActive(false);
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y);
        _action?.Invoke();
        
        var delta = 1f;
        
        while (delta > 0)
        {
            _black.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, delta);
            delta -= Time.deltaTime / 2;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}