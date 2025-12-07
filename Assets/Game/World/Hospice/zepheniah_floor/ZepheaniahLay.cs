using System.Collections;
using UnityEngine;

public sealed class ZepheaniahLay : MonoBehaviour, IUsable
{
    [SerializeField]
    private BattleDataConfig _battleConfig;

    [SerializeField]
    private Replica[] _replicas_1, _replicas_2, _replicas_end_battle_attack, _replicas_end_battle_mercy;

    [SerializeField]
    private GameObject _view_1;
    
    [SerializeField]
    private GameObject _view_2;
    
    private void OnEnable()
    {
        StartCoroutine(AwaitCutscene_2());
    }

    public void Use()
    {
        StartCoroutine(AwaitCutscene());
    }

    private IEnumerator AwaitCutscene()
    {
        Player.Instance.enabled = false;

        var isEnd = false;
        DialogueWindow.Open(_replicas_1, () =>
        {
            isEnd = true;
        });
        
        yield return new WaitUntil(() => isEnd);
        // Запускаем анимацию
        yield return new WaitForSeconds(1);
        
        isEnd = false;
        DialogueWindow.Open(_replicas_2, () =>
        {
            isEnd = true;
        });
        
        var startBattleAnimation = Instantiate(Resources.Load<StartBattleAnimation>("StartBattleAnimation"));
        startBattleAnimation.Init(11, new Vector2(-6.76f, -5.300001f), 
            () => FindAnyObjectByType<BattleController>().Init(_battleConfig.Data));
    }

    private IEnumerator AwaitCutscene_2()
    {
        Player.Instance.enabled = false;
        _view_1.SetActive(false);
        _view_2.SetActive(true);
        
        var isEnd = false;
        
        // Усли атаковали
        DialogueWindow.Open(_replicas_end_battle_attack, () => { isEnd = true; });

        yield return new WaitUntil(() => isEnd);
        
        // Анимация исчезновения
        Player.Instance.enabled = true;
    }
}