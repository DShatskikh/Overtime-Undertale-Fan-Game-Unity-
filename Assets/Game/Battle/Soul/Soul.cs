using System;
using System.Collections;
using UnityEngine;

public sealed class Soul : MonoBehaviour
{
    [SerializeField]
    private AudioSource _healSFX;

    [SerializeField]
    private AudioSource _damageSFX;
    
    [SerializeField]
    private float _speed;
    
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private bool _isInvulnerability;
    private bool _isUber;
    public static Soul Instance { get; private set; }
    public bool GetIsInvulnerability => _isInvulnerability;
    

    private void Awake()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        _isInvulnerability = false;
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * _speed;
    }

    public void Damage(float damage)
    {
        if (_isInvulnerability)
            return;
        
        if (_isUber)
            return;
        
        _damageSFX.Play();
        PlayerStats.Instance.HP -= damage;
        PlayerStats.Instance.UpdateHP();
        _animator.SetTrigger("Damage");
        StartCoroutine(AwaitInvulnerability());
    }

    private IEnumerator AwaitInvulnerability()
    {
        _isInvulnerability = true;
        yield return new WaitForSeconds(2);
        _isInvulnerability = false;
    }

    public void Heal(int health)
    {
        PlayerStats.Instance.HP += health;

        if (PlayerStats.Instance.HP > PlayerStats.Instance.MaxHP)
            PlayerStats.Instance.HP = PlayerStats.Instance.MaxHP;
        
        _healSFX.Play();
        
        PlayerStats.Instance.UpdateHP();
    }
    
    public void Uber()
    {
        _isUber = true;
        _animator.SetTrigger("Uber");
        Heal(20);
    }
}