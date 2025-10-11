using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class StartBattleAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject _black;

    [SerializeField]
    private Transform _target;

    private AudioSource _audioSource;
    private Soul _soul;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        transform.position = Player.Instance.transform.position;
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

        while (Vector3.Distance(_soul.transform.position, _target.position) >= 0.1f)
        {
            _soul.transform.position = Vector3.MoveTowards(_soul.transform.position, _target.position, Time.deltaTime * 5);
            yield return null;
        }

        _soul.transform.position = _target.position;
            
        Camera.main.gameObject.SetActive(false);
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
        
        var delta = 1f;
        
        while (delta > 0)
        {
            _black.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, delta);
            delta -= Time.deltaTime / 2;
            yield return null;
        }
        
        Soul.Instance.enabled = true;
        _black.SetActive(false);
    }
}