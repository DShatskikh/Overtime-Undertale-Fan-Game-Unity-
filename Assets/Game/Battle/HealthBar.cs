using System;
using UnityEngine;

public sealed class HealthBar : MonoBehaviour
{
    [SerializeField]
    private TextMesh _label;

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
    }
}