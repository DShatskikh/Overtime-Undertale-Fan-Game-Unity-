using System.Collections;
using UnityEngine;

public sealed class TimerDestroy : MonoBehaviour
{
    [SerializeField]
    private float _duration;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_duration);
        Destroy(gameObject);
    }
}