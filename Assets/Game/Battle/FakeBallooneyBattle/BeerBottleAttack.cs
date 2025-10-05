using System.Collections;
using UnityEngine;

public sealed class BeerBottleAttack : MonoBehaviour
{
    private const float SPEED = 6;

    private IEnumerator Start()
    {
        var direction = (Soul.Instance.transform.position - transform.position).normalized;
        
        while (true)
        {
            transform.position += direction * Time.deltaTime * SPEED;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Soul>())
        {
            Destroy(gameObject);
        }
    }
}