using System;
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
    
    private void Start()
    {
        if (SaveSystem.GetInt("Zepheaniah_State") == 4)
        {
            gameObject.SetActive(false);
            return;
        }
    }
    
    private void InstanceOnEndBattle()
    {
        if (SaveSystem.GetInt("Zepheaniah_State") == 3) // Kill
            StartCoroutine(AwaitCutscene_Kill());
        else if (SaveSystem.GetInt("Zepheaniah_State") == 2) // Mercy
            StartCoroutine(AwaitCutscene_Mercy());
    }

    public void Use()
    {
        if (SaveSystem.GetInt("Zepheaniah_State") == 0)
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
        _view_1.GetComponent<Animator>().SetTrigger("Up");
        yield return new WaitForSeconds(1);
        
        isEnd = false;
        DialogueWindow.Open(_replicas_2, () =>
        {
            isEnd = true;
        });
        
        yield return new WaitUntil(() => isEnd);
        
        var startBattleAnimation = Instantiate(Resources.Load<StartBattleAnimation>("StartBattleAnimation"));
        startBattleAnimation.Init(11, new Vector2(-6.76f, -5.300001f), 
            () =>
            {
                FindAnyObjectByType<BattleController>().Init(_battleConfig.Data);
                BattleController.Instance.EndBattle += InstanceOnEndBattle;
            });
    }

    private IEnumerator AwaitCutscene_Kill()
    {
        Player.Instance.enabled = false;
        _view_1.SetActive(false);
        _view_2.SetActive(true);
        
        var isEnd = false;
        DialogueWindow.Open(_replicas_end_battle_attack, () => { isEnd = true; });
        yield return new WaitUntil(() => isEnd);

        var delta = 0f;
        while (delta < 1)
        {
            delta += Time.deltaTime;
            _view_2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - delta);
            yield return null;
        }
        
        Player.Instance.enabled = true;
        SaveSystem.SetInt("Zepheaniah_State", 4);
        gameObject.SetActive(false);
    }

    private IEnumerator AwaitCutscene_Mercy()
    {
        Player.Instance.enabled = false;
        _view_1.SetActive(false);
        _view_2.SetActive(true);
        
        var isEnd = false;
        DialogueWindow.Open(_replicas_end_battle_mercy, () => { isEnd = true; });
        yield return new WaitUntil(() => isEnd);
        
        var delta = 0f;
        while (delta < 1)
        {
            delta += Time.deltaTime;
            _view_2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - delta);
            yield return null;
        }
        
        Player.Instance.enabled = true;
        SaveSystem.SetInt("Zepheaniah_State", 4);
        gameObject.SetActive(false);
    }
}