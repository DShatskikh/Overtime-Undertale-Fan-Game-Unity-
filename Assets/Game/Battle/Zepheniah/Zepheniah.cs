using System.Collections;
using UnityEngine;

public sealed class Zepheniah : Enemy
{
    private bool _isKill;

    public override void Init(BattleController battleController)
    {
        _battleController = battleController;
        _battleController.BattleApproachMessage = "* Here comes Zepheniah.";
    }

    public override IEnumerator AwaitEnemyTurn()
    {
        yield return _battleController.GetFrame.AwaitUpgradeSize(1.15f, 1.15f);
        Soul.Instance.gameObject.SetActive(true);
        Soul.Instance.transform.position = BattleController.Instance.transform.position + new Vector3(0, -2);
        Soul.Instance.enabled = true;

        yield return AwaitShowMessage();
        
        var attack = Instantiate(Resources.Load<Transform>("Attacks/Zephaniah Attack 1"), 
            new Vector3(BattleController.Instance.transform.position.x, transform.position.y), Quaternion.identity, transform);
        yield return new WaitForSeconds(5);
        
        Destroy(attack.gameObject);
        PlayerTurn();
    }
    
    public override IEnumerator AwaitAct(int act)
    {
        _actchoice = act + 1;
        Soul.Instance.gameObject.SetActive(false);
        
        if (_actchoice == 1)
        {
            _battleController.BattleApproachMessage = GetBaseInspected();
        }
        else if (_actchoice == 2)
        {
            if (Sparemeter == 20)
            {
                _battleController.BattleApproachMessage = "* You ask Zepheniah how\n  his day was.\n* He ignores your question.";
            }
                    
            if (Sparemeter == 15)
            {
                _battleController.BattleApproachMessage = "* You look at him eagerly,\n  as if you were waiting for\n  him to finish his story.";
            }
                    
            if (Sparemeter == 10)
            {
                _battleController.BattleApproachMessage = "* You're beginning to get\n  more and more invested.";
            }
                    
            if (Sparemeter == 5)
            {
                _battleController.BattleApproachMessage = "* You take out a piece of\n  paper and start to#  write down notes.";
            }
                    
            if (Sparemeter == 0)
            {
                _battleController.BattleApproachMessage = "* You raise your hand and ask\n  if this is going to be\n  on the test.";
            }
                    
            if (Sparemeter < 0)
            {
                _battleController.BattleApproachMessage = "* You want to ask how the\n  story continues, but you\n  realise it's already over.";
            }
        }
        else if (_actchoice == 3)
        {
            _battleController.BattleApproachMessage = "* You ask the ghost what's\n  that behind him.\n* He ignores you.";
        }
        else if (_actchoice == 4)
        {
            _battleController.BattleApproachMessage = "* You tell Zepheniah he's\n  very handsome.\n* He remains unfazed.";
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
            _battleController.BattleApproachMessage = "* Zepheniah's mood hasn't\n  changed a bit...or has it?\n* No, definitely not. Or yes?";
        }
        else if (_actchoice == 2)
        {
            Sparemeter -= 5;
            
            if (Sparemeter == 20)
            {
                _battleController.BattleApproachMessage = "* Zepheniah seems like he\n  needs to rant some more.";
            }
                    
            if (Sparemeter == 15)
            {
                _battleController.BattleApproachMessage = "* It feels like Zepheniah\n  has more to say.";
            }
                    
            if (Sparemeter == 10)
            {
                _battleController.BattleApproachMessage = "* Zepheniah doesn't seem\n  like he's done talking\n  already.";
            }
                    
            if (Sparemeter == 5)
            {
                _battleController.BattleApproachMessage = "* It appears that Zepheniah\n  still has one last thing\n  to say.";
            }
                    
            if (Sparemeter < 0)
            {
                _battleController.BattleApproachMessage = "* Zepheniah seems to have\n  finished his story now.";
            }
        }
        else if (_actchoice == 3)
        {
            _battleController.BattleApproachMessage = "* Zepheniah doesn't stop\n  looking you directly in the\n  eyes.";
        }
        else if (_actchoice == 4)
        {
            _battleController.BattleApproachMessage = "* Seems like someone\n  doesn't celebrate\n  Valentine's Day...";
        }

        StartCoroutine(AwaitEnemyTurn());
    }

    public override IEnumerator AwaitFight()
    {
        _isKill = true;
        StartCoroutine(BattleController.Instance.AwaitEndBattle());
        yield break;
    }

    public override IEnumerator AwaitDamage(int damage)
    {
        yield return null;
    }

    public override void End()
    {
        if (_isKill)
        {
            SaveSystem.SetInt("Zepheaniah_State", 3);
        }
        else
        {
            SaveSystem.SetInt("Zepheaniah_State", 2);
        }
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
        if (_actchoice == 0)
        {
            if (Sparemeter > 0)
                return new[] { "You are a fool for#coming here." };
            else
                return new[] { "I have nothing#else to add." };
        }
        else if (_actchoice == 1)
        {
            return new[] {"(Glares back at you)"};
        }
        else if (_actchoice == 2)
        {
            if (Sparemeter == 20)
            {
                return new[]
                {
                    "My useless sons\ndragged me\nto these states.",
                    "They convinced me\nto buy barren\ngravel pits.",
                    "I dreamed of making\na fortune there,\nby manufacturing\nweapons!"
                };
            }
            if (Sparemeter == 15)
            {
                return new[]
                {
                    "Yet it turned out\nto be nothing\nbut acres and acres#of useless, dry sand.",
                    "So I wasted my\nentire life trying\nto build my empire#in that desert.",
                    "But it was all for\nnothing, as I caught\nevery disease known#to man and died.",
                    "Since then, I've\nhated all weapons."
                };
            }
            
            if (Sparemeter == 10)
            {
                return new[]
                {
                    "I swore to haunt\nanyone who would\nuse firearms over\nmy bones.",
                    "Yet the morons of\nthe Badlands do\nnothing but that!",
                    "Shooting left and\nright, as if they\nhad nothing better\nto do!"
                };
            }
            
            if (Sparemeter == 5)
            {
                return new[]
                {
                    "I will be forced\nto stay on this\nawful planet\nfor all eternity!",
                    "Unless these fools\nare willing to give\nup their weapons."
                };
            }
            
            if (Sparemeter == 0)
            {
                return new[]
                {
                    "But that won't happen\nany time soon.",
                    "My suffering shall\nnever end, it seems."
                };
            }
            
            if (Sparemeter < 0)
            {
                return new[]
                {
                    "I have nothing\nelse to add."
                };
            }
        }
        else if (_actchoice == 3)
        {
            return new[] {"I can see behind me,\nyou know.\nI'm a ghost."};
        }
        else if (_actchoice == 4)
        {
            return new[] {"(Scoffs)"};
             // 0 "I have nothing#else to add."
        }

        return new []{ string.Empty };
    }
    
    private string Choose(string[] texts)
    {
        return texts[Random.Range(0, texts.Length)];
    }
}