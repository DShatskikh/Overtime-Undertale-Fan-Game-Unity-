using System;
using System.Collections;
using UnityEngine;

public sealed class DummyEnemy : Enemy
{
    public override void Init(BattleController battleController)
    {
        _battleController = battleController;
        _battleController.BattleApproachMessage = "* A dummy.";
        Health = MaxHealth;
    }
    
    public override IEnumerator AwaitDamage(int damage)
    {
        Health -=  damage;
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
    
    public override IEnumerator AwaitFight()
    {
        Soul.Instance.gameObject.SetActive(false);
        yield return _battleController.GetFrame.AwaitUpgradeSize(1.15f, 1.15f);
        Soul.Instance.gameObject.SetActive(true);
        Soul.Instance.transform.position = transform.position + new Vector3(0, -2);
        Soul.Instance.enabled = true;
        yield return AwaitShowMessage();
        yield return new WaitForSeconds(6);
        PlayerTurn();
    }
    
    public override IEnumerator AwaitAct(int act)
    {
        Soul.Instance.gameObject.SetActive(false);
        _actchoice = act + 1;
        
        yield return _battleController.BattleApproachMessage = _actchoice switch
        {
            1 => GetBaseInspected(),
            2 => "* You talk to the dummy.\n* Sani looks happy that\n  you didn't use violence.",
            3 => "* You tell the dummy that it\n  looks nice. Sani begins to\n  blush.",
            4 => "* You insult the Dummy.\n  Sani looks at you funny.",
        };

        _battleController.WriteStartMessage();
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (_battleController.IsGetWriteProcessing)
        {
            _battleController.ShowWriteAllLine();
            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        _battleController.StopWrite();
        
        _battleController.BattleApproachMessage = _actchoice switch
        {
            1 => "* Dummy was inspected.",
            2 => "* Dummy can't talk.",
            3 => "* Dummy doesn't feel joy.",
            4 => "* Dummy can't get mad.",
        };
        
        yield return _battleController.GetFrame.AwaitUpgradeSize(1.15f, 1.15f);
        Soul.Instance.gameObject.SetActive(true);
        Soul.Instance.transform.position = transform.position + new Vector3(0, -2);
        Soul.Instance.enabled = true;

        if (_actchoice == 1)
        {
            
        }
        else if (_actchoice == 2)
        {
            _sparemeter -= 10;
        }
        else if (_actchoice == 3)
        {
            _sparemeter -= 10;
        }
        else
        {
            _sparemeter -= 10;
        }
        
        yield return AwaitShowMessage();
        yield return new WaitForSeconds(6);

        PlayerTurn();
    }
    
    public override IEnumerator AwaitItem(string itemName)
    {
        Soul.Instance.gameObject.SetActive(false);
        yield return _battleController.GetFrame.AwaitUpgradeSize(1.15f, 1.15f);
        Soul.Instance.gameObject.SetActive(true);
        Soul.Instance.transform.position = transform.position + new Vector3(0, -2);
        Soul.Instance.enabled = true;

        yield return AwaitShowMessage();
        
        yield return new WaitForSeconds(6);

        PlayerTurn();
    }
    
    public override IEnumerator AwaitMercy()
    {
        Soul.Instance.gameObject.SetActive(false);

        if (IsYellowName)
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
            
            yield break;
        }

        yield return _battleController.GetFrame.AwaitUpgradeSize(1.15f, 1.15f);
        Soul.Instance.gameObject.SetActive(true);
        Soul.Instance.transform.position = transform.position + new Vector3(0, -2);
        Soul.Instance.enabled = true;

        yield return AwaitShowMessage();
        
        yield return new WaitForSeconds(6);

        PlayerTurn();
    }
    
    protected override string GetBaseInspected()
    {
        return $"* Dummy ATK {Attack} DEF {Defense}\n* Sani made it himself!\n* Doesn't attack or defend.";
    }
    
    protected override void PlayerTurn()
    {
        if (_sparemeter <= 0 || Health < 2)
        {
            if (Health < 2)
                _battleController.BattleApproachMessage = "* The Dummy's health\n  is low, which means\n  he can be spared.";
            else
                _battleController.BattleApproachMessage = "* Dummy can now be \"hired\".";

            IsYellowName = true;
        }
        
        _battleController.PlayerTurn();
    }
    
    protected override string GetMessage()
    {
        if (_sparemeter > 0)
            return "...";

        if (_actchoice == 4)
            return "...?";

        if (_actchoice == 1)
            return "...";

        if (_actchoice == 2)
            return "...!";

        if (_actchoice == 3)
            return "...!";

        if (_sparemeter <= 0)
            return "...:)";

        throw new Exception("Нету комментария");
    }

    public override void End()
    {
        SaveSystem.SetInt("Cutscene_Hospital", 4);
    }
}