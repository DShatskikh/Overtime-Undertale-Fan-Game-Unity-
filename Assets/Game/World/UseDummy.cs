using UnityEngine;

public sealed class UseDummy : MonoBehaviour, IUsable
{
    [SerializeField]
    private BattleDataConfig _battleData;

    [SerializeField]
    private Replica[] _replicas;
    
    public void Use()
    {
        if (SaveSystem.GetInt("Cutscene_Hospital") <= 3)
        {
            Player.Instance.enabled = false;
            var startBattleAnimation = Instantiate(Resources.Load<StartBattleAnimation>("StartBattleAnimation"));
            startBattleAnimation.Init(11, new Vector2(-6.76f, -5.300001f), () => FindAnyObjectByType<BattleController>().Init(_battleData.Data));
        }
        else
        {
            DialogueWindow.Open(_replicas, () =>
            {
                Player.Instance.enabled = true;
            }); 
        }
    }
}