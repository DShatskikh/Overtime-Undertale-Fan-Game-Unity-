using UnityEngine;

public sealed class SwenShell : Shell
{
    [SerializeField]
    private float _speed;
    
    protected override int _damage => 3;

    private void Update()
    {
        transform.position += new Vector3(0, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Soul soul))
        {
            soul.Damage(_damage);
        }
    }
}
    
