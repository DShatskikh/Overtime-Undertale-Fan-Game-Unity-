using System.Collections;
using UnityEngine;

public sealed class Cutscene_Hospital_6 : MonoBehaviour
{
    [SerializeField]
    private Replica[] _replicas;

    [SerializeField]
    private Animator _saniAnimator;

    [SerializeField]
    private Sparkles[] _sparkles;

    [SerializeField]
    private AudioClip _music;
    
    private void Start()
    {
        if (SaveSystem.GetInt("Cutscene_Hospital") > 6)
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
        Player.Instance.SetDirection(new Vector2(1, 0));
        MusicPlayer.Instance.Play(_music);
        
        // Говорим с игроком
        var isCloseDialogue = false;
        
        DialogueWindow.Open(_replicas, () =>
        {
            isCloseDialogue = true;
        });
        
        yield return new WaitUntil(() => isCloseDialogue);
        
        // Идем
        _saniAnimator.enabled = true;
        _saniAnimator.SetBool("IsMove", true);
        _saniAnimator.GetComponent<SpriteRenderer>().flipX = true;

        while (_saniAnimator.transform.localPosition.x < 1.63f)
        {
            yield return null;
            _saniAnimator.transform.position += new Vector3(Time.deltaTime * 2, 0);
        }

        foreach (var sparkle in _sparkles)
        {
            sparkle.Deactivate();
        }
        
        // Конец
        Player.Instance.enabled = true;
        SaveSystem.SetInt("Cutscene_Hospital", 7);
        Destroy(gameObject);
    }
}