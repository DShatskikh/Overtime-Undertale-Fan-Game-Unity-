using UnityEngine;

public sealed class SwenShell_2 : Shell
{
    [SerializeField]
    private float _speed;

    private float _timer = 1;
    
    protected override int _damage => 3;

    private void Update()
    {
        _timer -= Time.deltaTime;
        
        if (_timer > 0)
        {
            return;
        }
        
        transform.position += new Vector3(_speed * Time.deltaTime, 0);

        if (_timer < -1.5f)
            _timer = 999;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Soul soul))
        {
            soul.Damage(_damage);
        }
    }
}