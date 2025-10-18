using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SaveScreen : MonoBehaviour
{
    [SerializeField]
    private TextMesh _label;

    [SerializeField]
    private AudioSource _selectSFX;
    
    [SerializeField]
    private AudioSource _saveSFX;

    private bool _isLeft = true;
    private Soul _soul;
    private bool _isPlayedAnimation;
    private string _locationName;

    public void Init(string locationName)
    {
        _locationName = locationName;
    }
    
    private void Start()
    {
        _soul = Instantiate(Resources.Load<Soul>("Soul"));
        _soul.transform.position = transform.position + new Vector3(-1.61f, -0.544f) * 3;
        _soul.enabled = false;
        
        var time = SaveSystem.GetFloat("Time", GameTimer.Instance.GetTime);
        _label.text = $"{SaveSystem.GetString("PlayerName", "Miss")}   LV1     {(int)time - (int)time % 60}:{(int)time % 60}\n";
        _label.text += $"{SaveSystem.GetString("Location", _locationName)}\n\n";
        _label.text += " Save       Return";
    }

    private void Update()
    {
        if (_isPlayedAnimation)
            return;

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _isLeft = !_isLeft;
            _soul.transform.position = transform.position + (_isLeft ? new Vector3(-1.61f, -0.544f) * 3f : new Vector3(0.14f, -0.544f) * 3f);
            _selectSFX.Play();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isLeft)
            {
                _isPlayedAnimation = true;
                _saveSFX.Play();
                StartCoroutine(AwaitAnimation());
            }
            else
            {
                Destroy(gameObject);
                Destroy(_soul.gameObject);
                Player.Instance.enabled = true;
            }
        }
    }

    private IEnumerator AwaitAnimation()
    {
        Destroy(_soul.gameObject);
        _label.color = Color.yellow;
        _label.text = $"Denis   LV1     {(int)GameTimer.Instance.GetTime - (int)GameTimer.Instance.GetTime % 60}:{(int)GameTimer.Instance.GetTime % 60}\n";
        _label.text += $"{_locationName}\n\n";
        _label.text += " File saved";
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        
        SaveSystem.SetBool("IsHaveSavingUsableSaveStar", true);
        SaveSystem.SetFloat("PositionX", Player.Instance.transform.position.x);
        SaveSystem.SetFloat("PositionY", Player.Instance.transform.position.y);
        SaveSystem.SetInt("SceneIndex", SceneManager.GetActiveScene().buildIndex);
        SaveSystem.SetString("Location", _locationName);
        SaveSystem.SetFloat("Time", GameTimer.Instance.GetTime);
        SaveSystem.Save();
        
        Destroy(gameObject);
        Player.Instance.enabled = true;
    }
}