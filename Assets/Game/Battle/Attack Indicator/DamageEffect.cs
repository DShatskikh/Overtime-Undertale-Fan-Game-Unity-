using UnityEngine;

public sealed class DamageEffect : MonoBehaviour
{
    [SerializeField]
    private TextMesh _label;

    [SerializeField]
    private EnemyHealthBar _enemyHealth;
    
    public void Init(int damage, float health, float maxHealth)
    {
        if (damage == 0)
        {
            _label.text = "MISS POULING";
            _label.color = Color.white;
        }
        else
        {
            _label.text = damage.ToString();
            _label.color = Color.red;
        }
        
        _enemyHealth.ChangeHealth(health, maxHealth);
    }
}