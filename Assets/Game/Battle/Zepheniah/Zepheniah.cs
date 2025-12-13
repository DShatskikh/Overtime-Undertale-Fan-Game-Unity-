using System.Collections;
using UnityEngine;

public sealed class Zepheniah : Enemy
{
    public override void Init(BattleController battleController)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator AwaitAct(int act)
    {
        _actchoice = act + 1;
        Soul.Instance.gameObject.SetActive(false);
        
        if (_actchoice == 1)
        {
            yield return _battleController.BattleApproachMessage = "* Sven thinks you're\n  judging him for being\n  dirty.";
        }
        else if (_actchoice == 2)
        {
            yield return _battleController.BattleApproachMessage = "* Sven is still angry.";
        }
        else if (_actchoice == 3)
        {
            Sparemeter -= 5;
            
            if (Sparemeter == 10)
            {
                yield return _battleController.BattleApproachMessage = "* Sven has calmed down\n  a bit.";
            }
        
            if (Sparemeter == 5)
            {
                yield return _battleController.BattleApproachMessage = "* Even though you can't see\n  his face, Sven appears to\n  smile.";
            }
            
            if (Sparemeter < 0)
            {
                yield return _battleController.BattleApproachMessage = "* Sven admires himself in his\n  shiny armor.";
            }
        }
        else if (_actchoice == 4)
        {
            if (Sparemeter < 15 && Sparemeter != 0)
                Sparemeter += 5;
            
            yield return _battleController.BattleApproachMessage = "* Sven seems even more upset.";
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
            yield return _battleController.BattleApproachMessage = "* You pick up some dirt from\n  the ground and smear it all\n  over Sven.";
        }
        else if (_actchoice == 2)
        {
            yield return _battleController.BattleApproachMessage = Choose(
                new[] { "* You read a passage from\n  the little book of calm.\n* But Sven didn't listen.",
                    "* You try to calm Sven down.\n* He ignores you and tries to\n  dust off his shoulders."});
        }
        else if (_actchoice == 3)
        {
            if (Sparemeter == 10)
            {
                yield return _battleController.BattleApproachMessage = "* You use a broom to\n  sweep off all the dirt#  you put on Sven.";
            }
        
            if (Sparemeter == 5)
            {
                yield return _battleController.BattleApproachMessage = "* You put wax on the rag and\n  polish Sven's armor until\n  it's shiny again.";
            }
            
            if (Sparemeter == 0)
            {
                yield return _battleController.BattleApproachMessage = "* You use a wet rag to clean\n  Sven's armor until you can\n  see your face in it.";
            }
            
            if (Sparemeter < 0)
            {
                yield return _battleController.BattleApproachMessage = "* Your hands slip off of\n  Sven's armor, as it's\n  already clean.";
            }
        }
        else if (_actchoice == 4)
        {
            yield return _battleController.BattleApproachMessage = "* Sven seems even more upset.";
        }

        StartCoroutine(AwaitEnemyTurn());
    }

    public override void End()
    {
        throw new System.NotImplementedException();
    }

    protected override void PlayerTurn()
    {
        throw new System.NotImplementedException();
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
        else if (_actchoice == 2)
        {
            return new[] {"I can see behind me,\nyou know.\nI'm a ghost."};
        }
        else if (_actchoice == 3)
        {
            return new[] {"(Scoffs)"};
        }
        else if (_actchoice == 4)
        {
            return new[] {"(Glares back at you)"}; // 0 "I have nothing#else to add."
        }

        return new []{ string.Empty };
    }
    
    private string Choose(string[] texts)
    {
        return texts[Random.Range(0, texts.Length)];
    }
}