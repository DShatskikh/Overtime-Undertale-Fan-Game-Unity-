using UnityEngine;

public sealed class CryingCatShell_1 : Shell
{
    [SerializeField]
    private float _speed;

    protected override int _damage => 3;

    private void Update()
    {
        transform.position -= new Vector3(0, _speed * Time.deltaTime);
    }
}