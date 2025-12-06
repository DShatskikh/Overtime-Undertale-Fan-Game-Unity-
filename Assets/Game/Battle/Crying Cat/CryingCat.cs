using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class CryingCat : Enemy
{
    public override void Init(BattleController battleController)
    {
        _battleController = battleController;
        _battleController.BattleApproachMessage = "* The floor is wet from\n  Crying Cat's tears, which\n  makes you slip and fall.";
        Health = MaxHealth;
        
        XP = Random.Range(3, 5);
        Australium = Random.Range(3, 4);
    }

    public override IEnumerator AwaitFight()
    {
        StartCoroutine(AwaitEnemyTurn());
        yield return null;
    }

    public override IEnumerator AwaitEnemyTurn()
    {
        yield return _battleController.GetFrame.AwaitUpgradeSize(1.15f, 1.15f);
        Soul.Instance.gameObject.SetActive(true);
        Soul.Instance.transform.position = BattleController.Instance.transform.position + new Vector3(0, -2);
        Soul.Instance.enabled = true;
        
        var attack = Instantiate(Resources.Load<CryingCatAttack_1>("Attacks/Crying Cat Attack 1"), transform);
        yield return new WaitForSeconds(6);
        
        Destroy(attack.gameObject);
        PlayerTurn();
    }

    public override IEnumerator AwaitAct(int act)
    {
        _actchoice = act + 1;
        Soul.Instance.gameObject.SetActive(false);

        if (_actchoice != 4)
        {
            yield return _battleController.BattleApproachMessage = _actchoice switch
            {
                1 => GetBaseInspected(),
                2 => "* You make a bad cat pun.\n  Crying Cat does not care\n  for it.",
                3 =>  "* You spray Crying Cat with\n  a bottle of water. She does\n  not even acknowledge it.",
            };  
        }
        else
        {
            _sparemeter -= 5;
            _actchoice = 3;

            if (_sparemeter == 5)
            {
                yield return _battleController.BattleApproachMessage = "* You reach out and pet\n  Crying Cat's head.";
            }
            else if (_sparemeter == 0)
            {
                yield return _battleController.BattleApproachMessage = "* You scratch Crying Cat\n  behind the ears.";
            }
            else
            {
                yield return _battleController.BattleApproachMessage = "* Crying Cat bites your\n  finger, but out of love.\n* Enough petting.";
            }
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
        
        if (_actchoice != 4)
        {
            yield return _battleController.BattleApproachMessage = _actchoice switch
            {
                1 => GetBaseInspected(),
                2 => "* Crying Cat plays with\n  a yarn ball for a bit,\n  but it rolls away.",
                3 => "* Crying Cat is wet from both\n  the water and her tears.",
            };  
        }
        else
        {
            _sparemeter -= 5;
            _actchoice = 3;

            if (_sparemeter == 5)
            {
                yield return _battleController.BattleApproachMessage = "* Crying Cat's tears seem to\n  be drying up.";
            }
            else if (_sparemeter == 0)
            {
                yield return _battleController.BattleApproachMessage = "* Crying Cat purrs softly.\n* Seems like she's happy!";
            }
            else
            {
                yield return _battleController.BattleApproachMessage = "* Crying Cat bites your\n  finger, but out of love.\n* Enough petting.";
            }
        }
        
        StartCoroutine(AwaitEnemyTurn());
    }

    public override void End()
    {
        if (Health <= 0)
        {
            SaveSystem.SetBool("IsDummyKilled", true);
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
                _battleController.BattleApproachMessage = "* Crying Cat collapses in her\n  own tears.";
            else if (Health < 2)
                _battleController.BattleApproachMessage = "* Crying Cat shivers and\n  refuses to look at you.";
            else
                _battleController.BattleApproachMessage = "* Crying Cat purrs so loud,\n  the walls are beginning to\n  shake.";

            if (_sparemeter <= 0)
                _battleController.BattleApproachMessage = "* Crying Cat was hired as a\n  mice hunter for an old\n  lady.";
            
            IsYellowName = true;
        }
        
        _battleController.PlayerTurn();
    }

    protected override string GetMessage()
    {
        if (_sparemeter > 0 && _actchoice == 0)
        {
            return Choose();
        }
    
        if (_actchoice == 4)
        {
            return Choose();
        }
    
        if (_actchoice == 1)
        {
            return Choose();
        }
    
        if (_actchoice == 2)
        {
            return Choose();
        }
    
        if (_actchoice == 3)
        {
            if (_sparemeter > 4)
            {
                return "Meow?";
            }
            else
            {
                return "(Meow...)";
            }
        }
    
        if (_sparemeter <= 0 && _actchoice == 0)
        {
            return "(Purr...)";
        }

        throw new Exception("Нету комментария");
    }

    private string Choose()
    {
        return Random.Range(0, 4) switch
        {
            0 => "(Whine)",
            1 => "(Sigh)",
            2 => "(Sniff)",
            3 => "(Yelp)",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}