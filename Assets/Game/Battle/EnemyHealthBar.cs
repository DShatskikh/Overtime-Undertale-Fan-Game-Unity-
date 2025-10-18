using UnityEngine;

public sealed class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private Transform _healthProgress;

    public void ChangeHealth(float health, float maxHealth)
    {
        var progress = health / maxHealth;
        _healthProgress.localPosition = new Vector3(Mathf.Lerp(-0.5007f,  0f, progress), _healthProgress.localPosition.y);
        _healthProgress.localScale = new Vector3(Mathf.Lerp(0f, 1f, progress), _healthProgress.localScale.y);
    }
}