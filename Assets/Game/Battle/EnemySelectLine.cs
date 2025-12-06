using UnityEngine;

public sealed class EnemySelectLine : MonoBehaviour
{
    [SerializeField]
    private TextMesh _label;

    [SerializeField]
    private EnemyHealthBar _healthBar;
    
    public void Init(string enemyName, float health, float maxHealth, bool isYellowName)
    {
        _label.text = enemyName;
        _healthBar.ChangeHealth(health, maxHealth);

        _label.color = isYellowName ? Color.yellow : Color.white;
    }
}