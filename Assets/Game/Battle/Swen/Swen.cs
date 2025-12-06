using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class Swen : Enemy
{
    public override void Init(BattleController battleController)
    {
        _battleController = battleController;

        if (BattleController.Instance.GetEnemies.Count == 1)
        {
            _battleController.BattleApproachMessage =
                "* The floor is wet from\n  Crying Cat's tears, which\n  makes you slip and fall.";
        }
        else
        {
            _battleController.BattleApproachMessage = "* Irate Sven rants angrily.\n* A nearby cat";
        }

        Health = MaxHealth;
        
        XP = Random.Range(4, 6);
        Australium = Random.Range(3, 5);
    }

    protected override void ShowMessage(Action action = null)
    {
        var messageBox = Instantiate(Resources.Load<MessageEnemyBattle>("Message Enemy Battle"), 
            transform.position + new Vector3(1.21f, 2.93f), Quaternion.identity, transform);

        messageBox.Open(new []{ GetMessage()}, action );
    }
    
    public override IEnumerator AwaitEnemyTurn()
    {
        var activeEnemiesCount = BattleController.Instance.GetEnemies.Count(enemy =>
            !enemy.IsDead && !enemy.IsSpare);
        
        Debug.Log(activeEnemiesCount);
        
        if (activeEnemiesCount == 1)
            yield return _battleController.GetFrame.AwaitUpgradeSize(1.15f, 1.15f);
        else
            yield return _battleController.GetFrame.AwaitUpgradeSize(1.5f, 1.15f);
        
        Soul.Instance.gameObject.SetActive(true);
        Soul.Instance.transform.position = BattleController.Instance.transform.position + new Vector3(0, -2);
        Soul.Instance.enabled = true;

        yield return AwaitShowMessage();

        if (activeEnemiesCount != 1)
        {
            var attackSwenCat = Instantiate(Resources.Load<Transform>("Attacks/Swen Cat Attack 1"), transform);
            yield return new WaitForSeconds(6);
            Destroy(attackSwenCat.gameObject);
        }
        else
        {
            if (BattleController.Instance.GetEnemies.Count(enemy => !enemy.IsDead && !enemy.IsSpare && enemy == this) == 1)
            {
                var attack = Instantiate(Resources.Load<Transform>("Attacks/Swen Attack 1"), transform);
                yield return new WaitForSeconds(6);
                Destroy(attack.gameObject); 
            }
            else
            {
                var attack = Instantiate(Resources.Load<Transform>("Attacks/Crying Cat Attack 1"), transform);
                yield return new WaitForSeconds(6);
                Destroy(attack.gameObject);
            }
        }
        
        PlayerTurn();
    }
    
    public override IEnumerator AwaitAct(int act)
    {
        _actchoice = act + 1;
        Soul.Instance.gameObject.SetActive(false);
        
        if (_actchoice == 1)
        {
            yield return _battleController.BattleApproachMessage = "* Sven is still angry.";
        }
        else if (_actchoice == 2)
        {
            _sparemeter -= 5;
            
            if (_sparemeter == 10)
            {
                yield return _battleController.BattleApproachMessage = "* Sven has calmed down\n  a bit.";
            }
        
            if (_sparemeter == 5)
            {
                yield return _battleController.BattleApproachMessage = "* Even though you can't see\n  his face, Sven appears to\n  smile.";
            }
            
            if (_sparemeter < 0)
            {
                yield return _battleController.BattleApproachMessage = "* Sven admires himself in his\n  shiny armor.";
            }
        }
        else if (_actchoice == 3)
        {
            if (_sparemeter < 15 && _sparemeter != 0)
                _sparemeter += 5;
            
            yield return _battleController.BattleApproachMessage = "* Sven seems even more upset.";
        }
        else if (_actchoice == 4)
        {
            yield return _battleController.BattleApproachMessage = "* Sven thinks you're\n  judging him for being\n  dirty.";
        }
        
        _battleController.WriteStartMessage();
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (_battleController.IsGetWriteProcessing)
        {
            _battleController.ShowWriteAllLine();
            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        _battleController.StopWrite();

        if (_actchoice == 1)
        {
            yield return _battleController.BattleApproachMessage = Choose(
                new[] { "* You read a passage from\n  the little book of calm.\n* But Sven didn't listen.",
                    "* You try to calm Sven down.\n* He ignores you and tries to\n  dust off his shoulders."});
        }
        else if (_actchoice == 2)
        {
            if (_sparemeter == 10)
            {
                yield return _battleController.BattleApproachMessage = "* You use a broom to\n  sweep off all the dirt#  you put on Sven.";
            }
        
            if (_sparemeter == 5)
            {
                yield return _battleController.BattleApproachMessage = "* You put wax on the rag and\n  polish Sven's armor until\n  it's shiny again.";
            }
            
            if (_sparemeter == 0)
            {
                yield return _battleController.BattleApproachMessage = "* You use a wet rag to clean\n  Sven's armor until you can\n  see your face in it.";
            }
            
            if (_sparemeter < 0)
            {
                yield return _battleController.BattleApproachMessage = "* Your hands slip off of\n  Sven's armor, as it's\n  already clean.";
            }
        }
        else if (_actchoice == 3)
        {
            yield return _battleController.BattleApproachMessage = "* Sven seems even more upset.";
        }
        else if (_actchoice == 4)
        {
            yield return _battleController.BattleApproachMessage = "* You pick up some dirt from\n  the ground and smear it all\n  over Sven.";
        }

        StartCoroutine(AwaitEnemyTurn());
    }

    public override void End()
    {
        if (Health <= 0)
        {
            SaveSystem.SetBool("IsSwenKilled", true);
            
            var enemiesKilled = SaveSystem.GetInt("EnemiesKilled");
            SaveSystem.SetInt("EnemiesKilled", enemiesKilled + 1);
        }
        else
        {
            var enemiesSpared = SaveSystem.GetInt("EnemiesSpared");
            SaveSystem.SetInt("EnemiesSpared", enemiesSpared + 1);
        }
    }

    protected override void PlayerTurn()
    {
        if (_sparemeter <= 0 || Health < 2)
        {
            if (Health <= 0)
                _battleController.BattleApproachMessage = "* Sven shatters to the floor.";
            else if (Health < 2)
                _battleController.BattleApproachMessage =
                    "* Sven realizes he can only\n  complain when he's alive,\n  so he spares you.";
            else
                _battleController.BattleApproachMessage = "* Sven's grin is as bright as\n  his armor now.";

            IsYellowName = true;
        }
        

        _battleController.PlayerTurn();
    }

    protected override string GetMessage()
    {
        if (_actchoice == 0)
        {
            if (_sparemeter > 0)
                return "Grmbl...";
            else
               return "I am so\npretty.";
        }
        else if (_actchoice == 1)
        {
            return  "No one\ncleans\nme...";
        }
        else if (_actchoice == 2)
        {
            if (_sparemeter > 9)
            {
                return "I hate\nyou\nstill...";
            }
        
            if (_sparemeter > 4)
            {
                return "This is\nnicer...";
            }
            else
            {
                return "I feel\na lot\nbetter!";
            }
        }
        else if (_actchoice == 3)
        {
            return "WHY\nWOULD\nYOU DO\nTHAT?!?";;
        }
        else if (_actchoice == 4)
        {
            return "Stop\nthat!";
        }

        return string.Empty;
    }
    
    private string Choose(string[] texts)
    {
        return texts[Random.Range(0, texts.Length)];
    }
}