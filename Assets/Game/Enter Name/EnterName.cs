using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class EnterName : MonoBehaviour
{
    [SerializeField]
    private TextMesh _line_1_TextMesh;

    [SerializeField]
    private TextMesh _line_2_TextMesh;

    [SerializeField]
    private TextMesh _yesAndNoTextMesh;

    [SerializeField]
    private GameObject _heart;

    [SerializeField]
    private GameObject _monitorLine;
    
    [SerializeField]
    private TextMesh _nameTextMesh;

    [SerializeField]
    private AudioSource _writeSFX;

    [SerializeField]
    private AudioSource _startGameSFX;
    
    [SerializeField]
    private AudioSource _selectSFX;
    
    private Coroutine _nameInputAnimationCoroutine;
    private Coroutine _enterNameCoroutine;

    private IEnumerator Start()
    {
        var text = "> Insert your codename (6 remaining).\n> Press \"Space\" to continue\n>";
        _line_1_TextMesh.gameObject.SetActive(true);
        _line_1_TextMesh.text = string.Empty;

        for (int i = 0; i < text.Length; i++)
        {
            _line_1_TextMesh.text += text[i];
            _line_1_TextMesh.text += '_';
            _writeSFX.Play();
            yield return new WaitForSeconds(0.1f);
            _line_1_TextMesh.text = _line_1_TextMesh.text.Remove(_line_1_TextMesh.text.Length - 1);
        }

        yield return new WaitForSeconds(1f);
        _line_1_TextMesh.text += "\n>";
        _writeSFX.Play();
        
        _enterNameCoroutine = StartCoroutine(AwaitEnterName());
    }

    private IEnumerator AwaitEnterName()
    {
        _nameTextMesh.gameObject.SetActive(true);
        _nameTextMesh.text = string.Empty;

        while (true)
        {
            _nameInputAnimationCoroutine ??= StartCoroutine(AwaitNameInputAnimation());

            _line_1_TextMesh.text = $"> Insert your codename ({6 - _nameTextMesh.text.Length} remaining).\n> Press \"Space\" to continue\n>";
            
            yield return new WaitUntil(() => Input.inputString != string.Empty);

            if (Input.GetKeyDown(KeyCode.Space) && _nameTextMesh.text.Length != 0 && (_nameTextMesh.text.Length != 1 && _nameTextMesh.text[0] != '_'))
            {
                StopNameInputAnimation();
                StartCoroutine(AwaitWriteLine2());
                StopCoroutine(_enterNameCoroutine);
                yield break;
            }

            if (Input.GetKey(KeyCode.Backspace))
            {
                if (_nameTextMesh.text.Length == 0 || (_nameTextMesh.text.Length == 1 && _nameTextMesh.text[^1] == '_'))
                    continue;

                StopNameInputAnimation();
                _nameTextMesh.text = _nameTextMesh.text.Remove(_nameTextMesh.text.Length - 1);
                yield return new WaitForSeconds(0.05f);
                continue;
            }

            if (Input.inputString.Length == 0 || Input.inputString[0] == default || Input.inputString[0] == ' ')
                continue;
            
            if (_nameTextMesh.text.Length == 0 || (_nameTextMesh.text[^1] != '_' && _nameTextMesh.text.Length < 6)
                                               || (_nameTextMesh.text[^1] == '_' && _nameTextMesh.text.Length < 7))
            {
                StopNameInputAnimation();
                _nameTextMesh.text += _nameTextMesh.text.Length == 0
                    ? char.ToUpper(Input.inputString[0])
                    : Input.inputString;
                _writeSFX.Play();
            }
        }
    }

    private IEnumerator AwaitNameInputAnimation()
    {
        while (true)
        {
            if (_nameTextMesh.text.Length >= 6)
                yield break;

            yield return new WaitForSeconds(0.75f);
            _nameTextMesh.text += '_';
            yield return new WaitForSeconds(0.75f);
            _nameTextMesh.text = _nameTextMesh.text.Remove(_nameTextMesh.text.Length - 1);
        }
    }

    private void StopNameInputAnimation()
    {
        if (_nameTextMesh.text.Length != 0 && _nameTextMesh.text[^1] == '_')
        {
            _nameTextMesh.text = _nameTextMesh.text.Remove(_nameTextMesh.text.Length - 1);
        }

        if (_nameInputAnimationCoroutine != null)
            StopCoroutine(_nameInputAnimationCoroutine);

        _nameInputAnimationCoroutine = null;
    }

    private IEnumerator AwaitWriteLine2()
    {
        var text = "> Is this name correct?\n> ";
        _line_2_TextMesh.gameObject.SetActive(true);
        _line_2_TextMesh.text = string.Empty;

        for (int i = 0; i < text.Length; i++)
        {
            _line_2_TextMesh.text += text[i];
            _line_2_TextMesh.text += '_';
            _writeSFX.Play();
            yield return new WaitForSeconds(0.1f);
            _line_2_TextMesh.text = _line_2_TextMesh.text.Remove(_line_2_TextMesh.text.Length - 1);
        }

        StartCoroutine(AwaitSelected());
    }

    private IEnumerator AwaitSelected()
    {
        var text = "     Yes         No";
        _yesAndNoTextMesh.gameObject.SetActive(true);
        _yesAndNoTextMesh.text = string.Empty;

        for (int i = 0; i < text.Length; i++)
        {
            _yesAndNoTextMesh.text += text[i];
            _yesAndNoTextMesh.text += '_';
            _writeSFX.Play();
            yield return new WaitForSeconds(0.1f);
            _yesAndNoTextMesh.text = _yesAndNoTextMesh.text.Remove(_yesAndNoTextMesh.text.Length - 1);
        }
        
        _heart.SetActive(true);
        var isLeft = true;
        _heart.transform.position = new Vector3(-4.82f, _heart.transform.position.y);

        while (true)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isLeft)
                {
                    _heart.SetActive(false);
                    _monitorLine.SetActive(false);
                    GetComponent<Animator>().SetTrigger("Transition");
                    _nameTextMesh.color = new Color32(14, 2, 10, 255);

                    var delta = 1f;
                    
                    while (delta > 0)
                    {
                        var texts = new[]
                        {
                            _line_1_TextMesh,
                            _line_2_TextMesh,
                            _yesAndNoTextMesh
                        };
                        
                        foreach (var textMesh in texts)
                        {
                            var color = textMesh.color;
                            color.a = delta;
                            textMesh.color = color;
                        }

                        yield return null;
                        delta -= Time.deltaTime / 9;
                    }
                    
                    MusicPlayer.Instance.Stop();
                    _startGameSFX.Play();
                    yield return new WaitForSeconds(6);
                    SceneManager.LoadScene(2);
                }
                else
                {
                    _line_2_TextMesh.gameObject.SetActive(false);
                    _yesAndNoTextMesh.gameObject.SetActive(false);
                    _nameTextMesh.text = string.Empty;
                    _enterNameCoroutine = StartCoroutine(AwaitEnterName());
                    _heart.SetActive(false);
                }
                
                yield break;
            }
            
            if (Input.GetKeyDown(KeyCode.RightArrow) && isLeft)
            {
                isLeft = false;
                _heart.transform.position = new Vector3(-1.04f, _heart.transform.position.y);
                _selectSFX.Play();
                continue;
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !isLeft)
            {
                isLeft = true;
                _heart.transform.position = new Vector3(-4.882f, _heart.transform.position.y);
                _selectSFX.Play();
            }
        }
    }
}