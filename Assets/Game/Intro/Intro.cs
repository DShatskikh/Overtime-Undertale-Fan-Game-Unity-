using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [SerializeField]
    private TextMesh _label;

    [SerializeField]
    private SpriteRenderer _picture;

    [SerializeField]
    private SpriteRenderer _bigPicture;
    
    [SerializeField]
    private AudioSource _writeSFX;

    [SerializeField]
    private GameObject _logo;
    
    [SerializeField]
    private Sprite[] _slides;
    
    [SerializeField, TextArea]
    private string[] _texts;
    
    private Coroutine _coroutine;

    private void Start()
    {
        _coroutine = StartCoroutine(AwaitIntro());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(8);
        }
    }

    private IEnumerator AwaitIntro()
    {
         // Слайд 1 (Red and Blue)
        StartCoroutine(AwaitWrite(_texts[0]));
        yield return new WaitForSeconds(9);
        yield return AwaitHide();

        // Слайд 2 (Two mann)
        _picture.sprite = _slides[1];
        yield return AwaitShow();
        StartCoroutine(AwaitWrite(_texts[1]));
        yield return new WaitForSeconds(9);
        yield return AwaitHide();

        // Слайд 3 ()
        _picture.sprite = _slides[2];
        yield return AwaitShow();
        StartCoroutine(AwaitWrite(_texts[2]));
        yield return new WaitForSeconds(8);
        yield return AwaitHide();

        // Слайд 4 ()
        yield return AwaitShow();
        StartCoroutine(AwaitWrite(_texts[3]));
        yield return new WaitForSeconds(7);
        yield return AwaitHide();

        // Слайд 5 (Dark)
        StartCoroutine(AwaitWrite(_texts[4]));
        yield return new WaitForSeconds(6);

        // Слайд 6 (Rock)
        _picture.sprite = _slides[3];
        yield return AwaitShow();
        StartCoroutine(AwaitWrite(_texts[5]));
        yield return new WaitForSeconds(8);
        yield return AwaitHide();

        // Слайд 7 (Road)
        _picture.sprite = _slides[4];
        yield return AwaitShow();
        StartCoroutine(AwaitWrite(_texts[6]));
        yield return new WaitForSeconds(8);
        yield return AwaitHide();

        // Слайд 8 (Shot)
        _picture.sprite = _slides[5];
        yield return AwaitShow();
        yield return new WaitForSeconds(6);
        yield return AwaitHide();

        // Слайд 9 (Start fly)
        _picture.sprite = _slides[6];
        yield return AwaitShow();
        yield return new WaitForSeconds(7);
        yield return AwaitHide();
        
        // Слайд 10 (Flying)
        _picture.sprite = _slides[7];
        yield return AwaitShow();
        yield return new WaitForSeconds(7);
        yield return AwaitHide();
        
        // Слайд 11 (Landed)
        _picture.gameObject.SetActive(false);
        _bigPicture.gameObject.SetActive(true);
        _bigPicture.transform.localPosition = new Vector3(_bigPicture.transform.localPosition.x, 4.89f);
        
        var alpha = 0f;

        while (alpha < 1)
        {
            _bigPicture.color = new Color(1, 1, 1, alpha);
            yield return null;
            alpha += Time.deltaTime / 0.5f;
        }
        
        yield return new WaitForSeconds(5);

        var delta = 0f;
        
        while (delta < 1)
        {
            _bigPicture.transform.localPosition = new Vector3(_bigPicture.transform.localPosition.x, 
                Mathf.Lerp(4.89f, -5, delta));
            yield return null;
            delta += Time.deltaTime / 12;
        }

        yield return new WaitForSeconds(9);
        MusicPlayer.Instance.Stop();
        yield return new WaitForSeconds(2);
        
        alpha = 1f;

        while (alpha > 0)
        {
            _bigPicture.color = new Color(1, 1, 1, alpha);
            yield return null;
            alpha -= Time.deltaTime;
        }
        
        // Лого
        _label.gameObject.SetActive(false);
        _picture.gameObject.SetActive(false);
        _bigPicture.gameObject.SetActive(false);
        _logo.gameObject.SetActive(true);
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene(8);
    }

    private IEnumerator AwaitWrite(string text)
    {
        _label.text = string.Empty;

        for (int i = 0; i < text.Length; i++)
        {
            _label.text += text[i];
            _writeSFX.Play();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator AwaitHide()
    {
        var alpha = 1f;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime / 0.5f;
            _picture.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }
    
    private IEnumerator AwaitShow()
    {
        _label.text = string.Empty;
        var alpha = 0f;

        while (alpha < 1)
        {
            alpha += Time.deltaTime / 0.5f;
            _picture.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }
}
