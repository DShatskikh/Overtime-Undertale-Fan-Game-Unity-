using UnityEngine;

public abstract class Shell : MonoBehaviour
{
    protected abstract int _damage { get; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Soul soul))
        {
            Destroy(gameObject);
            soul.Damage(_damage);
        }
    }
}