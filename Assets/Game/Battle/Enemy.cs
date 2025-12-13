using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public string Name;
    public string Quip = "Sani made it himself!";
    public string Info = "Doesn't attack or defend.";
    public string[] Acts;
    public Attack[] Attacks;
    public bool IsRun;
    public bool IsYellowName;
    public int MaxHealth = 5;
    public int Health = 5;
    public int Sparemeter = 10;
    public int Defense = 0;
    public int Attack = 0;
    public int XP = 0;
    public int Australium = 0;

    protected int _actchoice;
    protected BattleController _battleController;
    public bool IsSpare { private set; get; }
    public bool IsDead { private set; get; }

    public abstract void Init(BattleController battleController);

    [ContextMenu("Damage")]
    public void DebugDamage()
    {
        Health = 1;
    }
    
    [ContextMenu("Spare")]
    public void DebugSpare()
    {
        IsYellowName = true;
    }
    
    public virtual IEnumerator AwaitDamage(int damage)
    {
        Health -=  damage;

        if (Health <= 0)
            IsDead = true;
        
        if (Health < 0)
            Health = 0;
        
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(2);
        
        if (Health > 0)
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        else
        {
            Instantiate(Resources.Load<Animator>("Dead Animation"), 
                transform.position, Quaternion.identity, transform);
            
            var delta = 0.5f;

            while (delta > 0f)
            {
                delta -= Time.deltaTime / 2;
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, delta);
                yield return null;
            }
        }
    }

    public virtual IEnumerator AwaitFight()
    {
        StartCoroutine(AwaitEnemyTurn());
        yield return null;
    }

    public abstract IEnumerator AwaitAct(int act);


    public virtual IEnumerator AwaitEnemyTurn()
    {
        yield return _battleController.GetFrame.AwaitUpgradeSize(1.15f, 1.15f);
        Soul.Instance.gameObject.SetActive(true);
        Soul.Instance.transform.position = transform.position + new Vector3(0, -2);
        Soul.Instance.enabled = true;
        yield return AwaitShowMessage();
        yield return new WaitForSeconds(6);

        PlayerTurn();
    }

    public virtual IEnumerator AwaitItem(string itemName)
    {
        StartCoroutine(AwaitEnemyTurn());
        yield return null;
    }

    public IEnumerator AwaitSpare(Action action)
    {
        Instantiate(Resources.Load<Animator>("Spare Animation"),
            transform.position, Quaternion.identity, transform);
            
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            
        var delta = 0.5f;

        while (delta > 0f)
        {
            delta -= Time.deltaTime * 2;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, delta);
            yield return null;
        }

        IsSpare = true;
        action.Invoke();
    }

    public virtual IEnumerator AwaitMercy()
    {
        StartCoroutine(AwaitEnemyTurn());
        yield return null;
    }
    
    public abstract void End();

    protected virtual string GetBaseInspected() => 
        $"* {Name} ATK {Attack} DEF {Defense}\n* {Quip}\n* {Info}";

    protected abstract void PlayerTurn();
    protected abstract string[] GetMessage();

    protected virtual void ShowMessage(Action action = null)
    {
        var messageBox = Instantiate(Resources.Load<MessageEnemyBattle>("Message Enemy Battle"), 
            transform.position + new Vector3(1.21f, 4.54f), Quaternion.identity, transform);

        messageBox.Open(GetMessage(), action );
    }

    protected IEnumerator AwaitShowMessage()
    {
        var selectedEnemy = this;

        if (selectedEnemy.IsDead || selectedEnemy.IsSpare)
        {
            selectedEnemy = null;
        }
        
        var isEnd = false;

        foreach (var enemy in _battleController.GetEnemies)
        {
            if (enemy != selectedEnemy && !enemy.IsSpare && !enemy.IsDead)
            {
                if (selectedEnemy == null)
                {
                    selectedEnemy = enemy;
                    continue;
                }
                
                enemy.ShowMessage();
            }
        }
        
        selectedEnemy.ShowMessage(() => isEnd = true);
        yield return new WaitUntil(() => isEnd);

        foreach (var messageEnemyBattle in FindObjectsByType<MessageEnemyBattle>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            Destroy(messageEnemyBattle.gameObject);
        }
    }
} 