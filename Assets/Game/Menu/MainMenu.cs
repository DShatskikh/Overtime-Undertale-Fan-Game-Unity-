using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MainMenu : MonoBehaviour
{
    [SerializeField]
    private TextMesh _infoLabel;
    
    [SerializeField]
    private TextMesh _continueLabel;
    
    [SerializeField]
    private TextMesh _resetLabel;
    
    private bool _isLeft = true;

    private void Start()
    {
        var time = SaveSystem.GetFloat("Time", GameTimer.Instance.GetTime);
        _infoLabel.text = $"{SaveSystem.GetString("PlayerName", "Miss")}   LV1     {(int)time - (int)time % 60}:{(int)time % 60}\n";
        _infoLabel.text += $"{SaveSystem.GetString("Location", "???")}";

        _continueLabel.color = Color.yellow;
    }

    private void Update()
    {
        if (_isLeft)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _isLeft = false;
                _continueLabel.color = Color.white;
                _resetLabel.color = Color.yellow;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _isLeft = true;
                _continueLabel.color = Color.yellow;
                _resetLabel.color = Color.white;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isLeft)
            {
                StartCoroutine(AwaitLoad());
            }
            else
            {
                SaveSystem.DeleteSave();
                SceneManager.LoadScene(1);
            }
        }
    }
    
    private IEnumerator AwaitLoad()
    {
        yield return SceneManager.LoadSceneAsync(SaveSystem.GetInt("SceneIndex", 4));
        Player.Instance.transform.position = new Vector3(SaveSystem.GetFloat("PositionX"), 
            SaveSystem.GetFloat("PositionY"));
    }
}
