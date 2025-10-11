using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class FakeBallooneyBattle : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _ballooneyView;

    [SerializeField]
    private Sprite _ballooneyNormal;
    
    [SerializeField]
    private Sprite _ballooneyAngry;
    
    [SerializeField]
    private Sprite _ballooneySad;

    [SerializeField]
    private AudioClip _normalSFX;
    
    [SerializeField]
    private AudioClip _angrySFX;

    [SerializeField]
    private GameObject _bottle;
    
    [SerializeField]
    private SpriteRenderer _whiteScreen;
    
    [SerializeField, TextArea]
    private string[] _replicas;
    
    [SerializeField, TextArea]
    private string[] _replicas1;
    
    [SerializeField, TextArea]
    private string[] _replicas2; // Подсказка
    
    [SerializeField, TextArea]
    private string _replicas3; // Готовит бутылку
    
    [SerializeField, TextArea]
    private string[] _replicas4; // После броска бутылки
    
    [SerializeField, TextArea]
    private string[] _replicas5; // Когда у игрока остался 1 хп
    
    [SerializeField, TextArea]
    private string[] _replicas6; // После убера
    
    [SerializeField, TextArea]
    private string _replicas7; // После неудачной попытке убить игрока
    
    [SerializeField, TextArea]
    private string[] _replicas8; // Реч во время перехода

    private GameObject _hint;
    private Coroutine _hintCoroutine;

    private void Awake()
    {
        if (Player.Instance)
            transform.position = Player.Instance.transform.position;

        if (!Soul.Instance)
        {
            Instantiate(Resources.Load<Soul>("Soul"));
            Soul.Instance.transform.localPosition = new Vector3(0, -2.2f);
        }

        StartCoroutine(AwaitCutscene());
    }

    
    private IEnumerator AwaitCutscene()
    {
        Soul.Instance.enabled = false;
        yield return new WaitForSeconds(3);
        Soul.Instance.enabled = true;

        var isEnd = false;
        MessageEnemyBattle.SFX = _normalSFX;
        var messageBox = Instantiate(Resources.Load<MessageEnemyBattle>("Message Enemy Battle Big"), 
            transform.position + new Vector3(1.46f, 3.41f), Quaternion.identity, transform);
        
        messageBox.Open(_replicas, () =>
        {
            isEnd = true;
        });

        yield return new WaitUntil(() => isEnd);
        
        isEnd = false;
        messageBox = Instantiate(Resources.Load<MessageEnemyBattle>("Message Enemy Battle"), 
            transform.position + new Vector3(1.46f, 3.41f), Quaternion.identity, transform);
        
        messageBox.Open(_replicas1, () =>
        {
            isEnd = true;
        });
        
        yield return new WaitUntil(() => isEnd);
        _hintCoroutine = StartCoroutine(AwaitHint());
        
        isEnd = false;
        messageBox = Instantiate(Resources.Load<MessageEnemyBattle>("Message Enemy Battle"), 
            transform.position + new Vector3(1.46f, 3.41f), Quaternion.identity, transform);
        
        messageBox.Open(_replicas2, () =>
        {
            isEnd = true;
        });

        yield return new WaitUntil(() => isEnd);

        if (_hintCoroutine != null)
        {
            StopCoroutine(_hintCoroutine);
            Destroy(_hint);
        }
        
        _bottle.SetActive(true);

        yield return new WaitForSeconds(1);
        isEnd = false;
        messageBox = Instantiate(Resources.Load<MessageEnemyBattle>("Message Enemy Battle"), 
            transform.position + new Vector3(1.46f, 3.41f), Quaternion.identity, transform);
        
        messageBox.Open(new []{ _replicas3 }, () =>
        {
            isEnd = true;
        });

        yield return new WaitUntil(() => isEnd);

        _bottle.SetActive(false);
        var bottle = Instantiate(Resources.Load<BeerBottleAttack>("Beer Bottle Attack"), 
            _bottle.transform.position, Quaternion.identity, transform);
        
        yield return new WaitWhile(() => bottle);
        _ballooneyView.sprite = _ballooneyAngry;
        
        MessageEnemyBattle.SFX = _angrySFX;
        isEnd = false;
        messageBox = Instantiate(Resources.Load<MessageEnemyBattle>("Message Enemy Battle"), 
            transform.position + new Vector3(1.46f, 3.41f), Quaternion.identity, transform);
        
        messageBox.Open(_replicas4, () =>
        {
            isEnd = true;
        });
        
        yield return new WaitUntil(() => isEnd);

        for (int i = 0; i < 5; i++)
        {
            Instantiate(Resources.Load<BeerBottleAttack>("Beer Bottle Attack"), 
                _ballooneyView.transform.position, Quaternion.identity, transform);

            yield return new WaitForSeconds(0.75f);
        }
        
        MessageEnemyBattle.SFX = _angrySFX;
        isEnd = false;
        messageBox = Instantiate(Resources.Load<MessageEnemyBattle>("Message Enemy Battle"), 
            transform.position + new Vector3(1.46f, 3.41f), Quaternion.identity, transform);
        
        messageBox.Open(_replicas5, () =>
        {
            isEnd = true;
        });
        
        yield return new WaitUntil(() => isEnd);
        yield return new WaitForSeconds(0.5f);
        Soul.Instance.Uber();
        yield return new WaitForSeconds(2f);
        _ballooneyView.sprite = _ballooneySad;
        
        MessageEnemyBattle.SFX = _normalSFX;
        isEnd = false;
        messageBox = Instantiate(Resources.Load<MessageEnemyBattle>("Message Enemy Battle"), 
            transform.position + new Vector3(1.46f, 3.41f), Quaternion.identity, transform);
        
        messageBox.Open(_replicas6, () =>
        {
            isEnd = true;
        });
        
        yield return new WaitUntil(() => isEnd);
        _ballooneyView.sprite = _ballooneyAngry;
        
        for (int i = 0; i < 4; i++)
        {
            Instantiate(Resources.Load<BeerBottleAttack>("Beer Bottle Attack"), 
                _ballooneyView.transform.position, Quaternion.identity, transform);

            yield return new WaitForSeconds(0.75f);
        }
        
        MessageEnemyBattle.SFX = _angrySFX;
        _ballooneyView.sprite = _ballooneySad;
        
        isEnd = false;
        messageBox = Instantiate(Resources.Load<MessageEnemyBattle>("Message Enemy Battle"), 
            transform.position + new Vector3(1.46f, 3.41f), Quaternion.identity, transform);
        
        messageBox.Open(new []{ _replicas7 }, () =>
        {
            isEnd = true;
        });
        
        yield return new WaitUntil(() => isEnd);
        
        messageBox = Instantiate(Resources.Load<MessageEnemyBattle>("Message Enemy Battle"), 
            transform.position + new Vector3(1.46f, 3.41f), Quaternion.identity, transform);
        
        messageBox.Open(_replicas8, () => { });

        _whiteScreen.transform.position = transform.position;
        _whiteScreen.gameObject.SetActive(true);
        
        MusicPlayer.Instance.Stop();
        
        var delta = 0f;
        
        while (delta < 1)
        {
            _whiteScreen.color = new Color(1, 1, 1, delta);
            delta += Time.deltaTime / 4;
            yield return null;
        }

        SceneManager.LoadScene(4);
    }

    private IEnumerator AwaitHint()
    {
        _hint = Instantiate(Resources.Load<GameObject>("Hint"), Soul.Instance.transform);
        
        for (int i = 0; i < 6; i++)
        {
            _hint.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            _hint.SetActive(false);
            
            if (i != 5)
                yield return new WaitForSeconds(0.5f);
        }
        
        Destroy(_hint);
        _hintCoroutine = null;
    }
}