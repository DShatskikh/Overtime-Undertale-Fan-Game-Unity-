using UnityEngine;

public sealed class TestStartBattle : MonoBehaviour
{
    [SerializeField]
    private BattleDataConfig _battleData;

    private void Start()
    {
        if (!FindAnyObjectByType<StartBattleAnimation>())
            return;
        
        FindAnyObjectByType<BattleController>().Init(_battleData.Data);
    }
}