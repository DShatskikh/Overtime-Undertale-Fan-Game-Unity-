using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class CryingCat : Enemy
{
    public override void Init(BattleController battleController)
    {
        _battleController = battleController;
        
        if (BattleController.Instance.GetEnemies.Count == 1)
        {
            _battleController.BattleApproachMessage = "* The floor is wet from\n  Crying Cat's tears, which\n  makes you slip and fall.";
        }
        
        Health = MaxHealth;
        
        XP = Random.Range(3, 5);
        Australium = Random.Range(3, 4);
    }

    protected override void ShowMessage(Action action = null)
    {
        var messageBox = Instantiate(Resources.Load<MessageEnemyBattle>("Message Enemy Battle"), 
            transform.position + new Vector3(1.21f, 0.92f), Quaternion.identity, transform);

        messageBox.Open(GetMessage(), action );
    }
    
    public override IEnumerator AwaitEnemyTurn()
    {
        yield return _battleController.GetFrame.AwaitUpgradeSize(1.15f, 1.15f);
        Soul.Instance.gameObject.SetActive(true);
        Soul.Instance.transform.position = BattleController.Instance.transform.position + new Vector3(0, -2);
        Soul.Instance.enabled = true;

        yield return AwaitShowMessage();
        
        var attack = Instantiate(Resources.Load<CryingCatAttack_1>("Attacks/Crying Cat Attack 1"), 
            new Vector3(BattleController.Instance.transform.position.x, transform.position.y), Quaternion.identity, transform);
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
            Sparemeter -= 5;
            _actchoice = 3;

            if (Sparemeter == 5)
            {
                _battleController.BattleApproachMessage = "* You reach out and pet\n  Crying Cat's head.";
            }
            else if (Sparemeter == 0)
            {
                _battleController.BattleApproachMessage = "* You scratch Crying Cat\n  behind the ears.";
            }
            else
            {
                _battleController.BattleApproachMessage = "* Crying Cat bites your\n  finger, but out of love.\n* Enough petting.";
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
            Sparemeter -= 5;
            _actchoice = 3;

            if (Sparemeter == 5)
            {
                _battleController.BattleApproachMessage = "* Crying Cat's tears seem to\n  be drying up.";
            }
            else if (Sparemeter == 0)
            {
                _battleController.BattleApproachMessage = "* Crying Cat purrs softly.\n* Seems like she's happy!";
            }
            else
            {
                _battleController.BattleApproachMessage = "* Crying Cat bites your\n  finger, but out of love.\n* Enough petting.";
            }
        }
        
        StartCoroutine(AwaitEnemyTurn());
    }

    public override void End()
    {
        
    }

    protected override void PlayerTurn()
    {
        if (Sparemeter <= 0 || Health < 2)
        {
            if (Health <= 0)
                _battleController.BattleApproachMessage = "* Crying Cat collapses in her\n  own tears.";
            else if (Health < 2)
                _battleController.BattleApproachMessage = "* Crying Cat shivers and\n  refuses to look at you.";
            else
                _battleController.BattleApproachMessage = "* Crying Cat purrs so loud,\n  the walls are beginning to\n  shake.";

            if (Sparemeter <= 0)
                _battleController.BattleApproachMessage = "* Crying Cat was hired as a\n  mice hunter for an old\n  lady.";
            
            IsYellowName = true;
        }
        
        _battleController.PlayerTurn();
    }

    protected override string[] GetMessage()
    {
        if (Sparemeter > 0 && _actchoice == 0)
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
            if (Sparemeter > 4)
            {
                return new[] {"Meow?"};
            }
            else
            {
                return new[] {"(Meow...)"};
            }
        }
    
        if (Sparemeter <= 0 && _actchoice == 0)
        {
            return new[] {"(Purr...)"};
        }

        throw new Exception("Нету комментария");
    }

    private string[] Choose()
    {
        return new[] {Random.Range(0, 4) switch
        {
            0 => "(Whine)",
            1 => "(Sigh)",
            2 => "(Sniff)",
            3 => "(Yelp)",
            _ => throw new ArgumentOutOfRangeException()
        }};
    }
}