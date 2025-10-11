using System.Collections;
using UnityEngine;

public sealed class BeerBottleAttack : MonoBehaviour
{
    private const float SPEED = 12;
    private const float ROTATION_SPEED = 200;

    [SerializeField]
    private GameObject _shark;
    
    private IEnumerator Start()
    {
        var direction = (Soul.Instance.transform.position - transform.position).normalized;
        
        while (true)
        {
            transform.position += direction * Time.deltaTime * SPEED;
            yield return null;
        }
    }

    private void Update()
    {
        transform.eulerAngles += Vector3.forward * ROTATION_SPEED * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Soul>(out var soul))
        {
            if (soul.GetIsInvulnerability)
                return;

            if (soul.GetHealth > 5)
                soul.Damage(5);
            else
                soul.Damage(soul.GetHealth - 1f);
            
            Instantiate(_shark, transform.position, Quaternion.identity, transform.parent);
            Destroy(gameObject); 
        }
    }
}