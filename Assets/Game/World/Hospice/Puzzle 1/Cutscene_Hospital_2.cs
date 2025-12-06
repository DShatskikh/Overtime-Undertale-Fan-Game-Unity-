using System.Collections;
using UnityEngine;

public sealed class Cutscene_Hospital_2 : MonoBehaviour
{
    [SerializeField]
    private Replica[] _replicas;

    [SerializeField]
    private Animator _saniAnimator;
    
    private void Start()
    {
        if (SaveSystem.GetInt("Cutscene_Hospital") != 1)
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
        
        DialogueWindow.Open(_replicas, () =>
        {
            isCloseDialogue = true;
        });
        
        yield return new WaitUntil(() => isCloseDialogue);
        
        // Идем
        _saniAnimator.enabled = true;
        _saniAnimator.CrossFade("Sani up move", 0);

        while (_saniAnimator.transform.localPosition.y < 10.27f)
        {
            yield return null;
            _saniAnimator.transform.position += new Vector3(0, Time.deltaTime * 2);
        }

        // Конец
        Player.Instance.enabled = true;
        SaveSystem.SetInt("Cutscene_Hospital", 2);
        Destroy(gameObject);
    }
}