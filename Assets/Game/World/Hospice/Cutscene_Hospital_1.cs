using System;
using System.Collections;
using UnityEngine;

public sealed class Cutscene_Hospital_1 : MonoBehaviour
{
    [SerializeField]
    private Replica[] _replicas;

    [SerializeField]
    private Animator _saniAnimator;

    [SerializeField]
    private GameObject _warning;

    private void Start()
    {
        if (SaveSystem.GetBool("Cutscene_Hospital_1"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.enabled = false;
            StartCoroutine(AwaitCutscene());
        }
    }

    private IEnumerator AwaitCutscene()
    {
        _saniAnimator.gameObject.SetActive(true);
        _saniAnimator.transform.localPosition = new Vector3(15f, _saniAnimator.transform.localPosition.y);
        
        // Идем
        _saniAnimator.SetBool("IsMove", true);

        while (_saniAnimator.transform.localPosition.x > 7.75f)
        {
            yield return null;
            _saniAnimator.transform.localPosition -= new Vector3(Time.deltaTime * 2, 0);
        }
        
        // Увидели игрока
        _saniAnimator.SetBool("IsMove", false);
        Player.Instance.SetDirection(new Vector2(1, 0));
        _warning.SetActive(true);
        yield return new WaitForSeconds(1);
        _warning.SetActive(false);
        
        // Бежим к игроку
        _saniAnimator.SetBool("IsMove", true);

        while (_saniAnimator.transform.position.x > 2.94f)
        {
            yield return null;
            _saniAnimator.transform.position -= new Vector3(Time.deltaTime * 4, 0);
        }
        
        _saniAnimator.SetBool("IsMove", false);
        
        // Говорим с игроком
        var isCloseDialogue = false;
        
        DialogueWindow.Open(_replicas, () =>
        {
            isCloseDialogue = true;
        });
        
        yield return new WaitUntil(() => isCloseDialogue);
        
        // Идем
        _saniAnimator.GetComponent<SpriteRenderer>().flipX = true;
        _saniAnimator.SetBool("IsMove", true);

        while (_saniAnimator.transform.position.x < 15)
        {
            yield return null;
            _saniAnimator.transform.position += new Vector3(Time.deltaTime * 2, 0);
        }

        Player.Instance.enabled = true;
        SaveSystem.SetBool("Cutscene_Hospital_1", true);
        Destroy(gameObject);
    }
}