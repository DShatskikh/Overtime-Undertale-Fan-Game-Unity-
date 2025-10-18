using UnityEngine;

public sealed class TranceCutscene : MonoBehaviour
{
    [SerializeField]
    private Replica[] _replicas;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Player>())
            return;
        
        Player.Instance.enabled = false;
        DialogueWindow.Open(_replicas, () =>
        {
            var startBattleAnimation = Instantiate(Resources.Load<StartBattleAnimation>("StartBattleAnimation"));
            startBattleAnimation.Init(3, new Vector2(0f, -2f), () => { });
        });
    }
}