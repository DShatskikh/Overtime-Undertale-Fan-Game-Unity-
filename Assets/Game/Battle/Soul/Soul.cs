using System;
using System.Collections;
using UnityEngine;

public sealed class Soul : MonoBehaviour
{
    [SerializeField]
    private AudioSource _healSFX;
    
    private float _health;
    private float _maxHealth;
    private Animator _animator;
    private bool _isInvulnerability;
    private bool _isUber;
    public static Soul Instance { get; private set; }
    public float GetHealth => _health;
    public float GetMaxHealth => _maxHealth;
    public bool GetIsInvulnerability => _isInvulnerability;
    public event Action<float, float> ChangeHealth;

    private void Awake()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _maxHealth = 20;
        _health = _maxHealth;
        ChangeHealth?.Invoke(_health, _maxHealth);
    }

    private void Update()
    {
        transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Time.deltaTime;
    }

    public void Damage(float damage)
    {
        if (_isInvulnerability)
            return;
        
        if (_isUber)
            return;
        
        _health -= damage;
        _animator.SetTrigger("Damage");
        StartCoroutine(AwaitInvulnerability());
        ChangeHealth?.Invoke(_health, _maxHealth);
    }

    private IEnumerator AwaitInvulnerability()
    {
        _isInvulnerability = true;
        yield return new WaitForSeconds(2);
        _isInvulnerability = false;
    }

    public void Heal(int health)
    {
        _health += health;

        if (_health > _maxHealth)
            _health = _maxHealth;
        
        _healSFX.Play();
        
        ChangeHealth?.Invoke(_health, _maxHealth);
    }
    
    public void Uber()
    {
        _isUber = true;
        _animator.SetTrigger("Uber");
        Heal(20);
    }
}