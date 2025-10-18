using System.Collections;
using UnityEngine;

public sealed class Cutscene_Hospital_4 : MonoBehaviour, IUsable
{
    [SerializeField]
    private Replica[] _replicasNoFight;
    
    [SerializeField]
    private Replica[] _replicasEndFight;
    
    [SerializeField]
    private Animator _saniAnimator;

    public void Use()
    {
        if (SaveSystem.GetInt("Cutscene_Hospital") <= 3)
        {
            Player.Instance.enabled = false;
        
            DialogueWindow.Open(_replicasNoFight, () =>
            {
                Player.Instance.enabled = true;
            });
        }
        else
        {
            Player.Instance.enabled = false;
        
            DialogueWindow.Open(_replicasEndFight, () =>
            {
                StartCoroutine(AwaitCutscene());
            });
        }
    }

    private IEnumerator AwaitCutscene()
    {
        // Идем
        _saniAnimator.enabled = true;
        _saniAnimator.CrossFade("Sani up move", 0);

        while (_saniAnimator.transform.localPosition.y < 6.73f)
        {
            yield return null;
            _saniAnimator.transform.position += new Vector3(0, Time.deltaTime * 2);
        }

        // Конец
        Player.Instance.enabled = true;
        SaveSystem.SetInt("Cutscene_Hospital", 5);
        Destroy(gameObject);
    }
}