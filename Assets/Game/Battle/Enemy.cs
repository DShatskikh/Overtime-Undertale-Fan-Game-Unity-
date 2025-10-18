using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public string Name;
    public string[] Acts;
    public bool IsRun;
    public bool IsYellowName;
    public int MaxHealth = 5;
    public int Health = 5;
    public int Defense = 0;
    public int Attack = 0;
    public int XP = 0;
    public int Australium = 0;

    protected int _sparemeter = 10;
    protected int _actchoice;
    protected BattleController _battleController;

    public abstract void Init(BattleController battleController);

    public abstract IEnumerator AwaitDamage(int damage);

    public abstract IEnumerator AwaitFight();

    public abstract IEnumerator AwaitAct(int act);

    public abstract IEnumerator AwaitItem(string itemName);

    public abstract IEnumerator AwaitMercy();
    public abstract void End();
    
    protected abstract string GetBaseInspected();
    protected abstract void PlayerTurn();
    protected abstract string GetMessage();

    private void ShowMessage()
    {
        var messageBox = Instantiate(Resources.Load<MessageEnemyBattle>("Message Enemy Battle"), 
            transform.position + new Vector3(1.21f, 4.54f), Quaternion.identity, transform);

        messageBox.Open(new []{GetMessage()}, () => { });
    }

    protected IEnumerator AwaitShowMessage()
    {
        var isEnd = false;

        foreach (var enemy in _battleController.GetEnemies)
        {
            if (enemy != this)
                enemy.ShowMessage();
        }
        
        var messageBox = Instantiate(Resources.Load<MessageEnemyBattle>("Message Enemy Battle"), 
            transform.position + new Vector3(1.21f, 4.54f), Quaternion.identity, transform);

        messageBox.Open(new []{GetMessage()}, () => isEnd = true);
        yield return new WaitUntil(() => isEnd);
    }
} 