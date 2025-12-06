using System.Collections;
using UnityEngine;

public sealed class Cutscene_Hospital_3 : MonoBehaviour
{
    [SerializeField]
    private Replica[] _replicas;
    
    [SerializeField]
    private Animator _saniAnimator;
    
    private void Start()
    {
        if (SaveSystem.GetInt("Cutscene_Hospital") > 2)
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
        // Говорим с игроком
        var isCloseDialogue = false;

        DialogueWindow.Open(_replicas, () => { isCloseDialogue = true; });

        yield return new WaitUntil(() => isCloseDialogue);
            
        // Идем
        _saniAnimator.enabled = true;
        _saniAnimator.GetComponent<SpriteRenderer>().flipX = true;
        _saniAnimator.SetBool("IsMove", true);

        while (_saniAnimator.transform.localPosition.x < 3.18f)
        {
            yield return null;
            _saniAnimator.transform.position += new Vector3(Time.deltaTime * 2, 0);
        }

        // Конец
        Player.Instance.enabled = true;
        SaveSystem.SetInt("Cutscene_Hospital", 2);
        Destroy(gameObject);
    }
}