using UnityEngine;

public sealed class HealthBar : MonoBehaviour
{
    [SerializeField]
    private TextMesh _label;

    [SerializeField]
    private Transform _healthProgress;
    
    private void Start()
    {
        Soul.Instance.ChangeHealth += OnChangeHealth;
    }

    private void OnDestroy()
    {
        Soul.Instance.ChangeHealth -= OnChangeHealth;
    }

    public void OnChangeHealth(float health, float maxHealth)
    {
        _label.text = $"{Soul.Instance.GetHealth}/{Soul.Instance.GetMaxHealth}";
        _healthProgress.localPosition = new Vector3(Mathf.Lerp(-0.3438525f,  0f, health / maxHealth), _healthProgress.localPosition.y);
        _healthProgress.localScale = new Vector3(Mathf.Lerp(0f, 0.6877051f, health / maxHealth), _healthProgress.localScale.y);
    }
}