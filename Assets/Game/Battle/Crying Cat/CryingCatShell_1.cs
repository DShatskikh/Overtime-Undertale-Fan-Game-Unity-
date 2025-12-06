using UnityEngine;

public sealed class CryingCatShell_1 : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    
    private void Update()
    {
        transform.position -= new Vector3(0, _speed * Time.deltaTime);
    }
}