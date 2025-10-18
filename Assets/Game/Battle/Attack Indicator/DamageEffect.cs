using UnityEngine;

public sealed class DamageEffect : MonoBehaviour
{
    [SerializeField]
    private TextMesh _label;

    [SerializeField]
    private EnemyHealthBar _enemyHealth;
    
    public void Init(string text, float health, float maxHealth)
    {
        _label.text = text;
        _enemyHealth.ChangeHealth(health, maxHealth);
    }
}