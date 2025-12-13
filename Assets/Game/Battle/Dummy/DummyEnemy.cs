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
        
        _battleController.BattleApproachMessage = _actchoice switch
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
            Sparemeter -= 10;
        }
        else if (_actchoice == 3)
        {
            Sparemeter -= 10;
        }
        else
        {
            Sparemeter -= 10;
        }
        
        yield return AwaitShowMessage();
        yield return new WaitForSeconds(6);

        PlayerTurn();
    }

    protected override void PlayerTurn()
    {
        if (Sparemeter <= 0 || Health < 2)
        {
            if (Health <= 0)
                _battleController.BattleApproachMessage = "* Dummy has been shot. Ow.";
            else if (Health < 2)
                _battleController.BattleApproachMessage = "* The Dummy's health\n  is low, which means\n  he can be spared.";
            else
                _battleController.BattleApproachMessage = "* Dummy can now be \"hired\".";

            IsYellowName = true;
        }
        
        _battleController.PlayerTurn();
    }
    
    protected override string[] GetMessage()
    {
        if (Sparemeter > 0)
            return new[] { "..." };

        if (_actchoice == 4)
            return new[] { "...?" };

        if (_actchoice == 1)
            return new[] { "..." };

        if (_actchoice == 2)
            return new[] { "...!" };

        if (_actchoice == 3)
            return new[] { "...!" };

        if (Sparemeter <= 0)
            return new[] { "...:)" };

        throw new Exception("Нужно добавить реплику");
    }

    public override void End()
    {
        if (Health <= 0)
        {
            SaveSystem.SetBool("IsDummyKilled", true);
        }
        
        SaveSystem.SetInt("Cutscene_Hospital", 4);
    }
}