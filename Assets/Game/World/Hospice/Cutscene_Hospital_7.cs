using System.Collections;
using UnityEngine;

public sealed class Cutscene_Hospital_7 : MonoBehaviour
{
    [SerializeField]
    private Replica[] _replicas_1, _replicas_2;

    [SerializeField]
    private Animator _saniAnimator;

    [SerializeField]
    private GameObject _danger;
    
    private void Start()
    {
        if (SaveSystem.GetInt("Cutscene_Hospital") > 7)
        {
            Destroy(gameObject);
        }
        
        _saniAnimator.CrossFade("Sani Up", 0);
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
        yield return new WaitForSeconds(1);
        
        //Реплика
        var isEndReplica = false;
        DialogueWindow.Open(_replicas_1, () =>
        {
            isEndReplica = true;
        });
        yield return new WaitUntil(() => isEndReplica);

        //Поворот
        yield return new WaitForSeconds(0.5f);
        _saniAnimator.CrossFade("Sani Right", 0);
        
        //Поворот
        yield return new WaitForSeconds(0.5f);
        _saniAnimator.CrossFade("Sani down", 0);
        
        //Предупреждение
        yield return new WaitForSeconds(0.5f);
        _danger.SetActive(true);

        //Пропало
        yield return new WaitForSeconds(1f);
        _danger.SetActive(false);
        
        //Реплика
        isEndReplica = false;
        DialogueWindow.Open(_replicas_2, () =>
        {
            isEndReplica = true;
        });
        
        yield return new WaitUntil(() => isEndReplica);
        
        // Идет
        _saniAnimator.CrossFade("Sani Left Move", 0);
        
        while (_saniAnimator.transform.localPosition.x > -1.969f)
        {
            yield return null;
            _saniAnimator.transform.position -= new Vector3(Time.deltaTime * 3, 0);
        }
        
        // Идет вверх
        _saniAnimator.CrossFade("Sani up move", 0);
        
        while (_saniAnimator.transform.localPosition.y < 6.13f)
        {
            yield return null;
            _saniAnimator.transform.position += new Vector3(0, Time.deltaTime * 3);
        }
        
        Player.Instance.enabled = true;
        SaveSystem.SetInt("Cutscene_Hospital", 7);
        Destroy(gameObject);
    }
}