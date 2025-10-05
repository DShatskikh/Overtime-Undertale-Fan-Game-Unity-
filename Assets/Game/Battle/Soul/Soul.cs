using UnityEngine;

public sealed class Soul : MonoBehaviour
{
    private Animator _animator;
    public static Soul Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Time.deltaTime;
    }

    public void Uber()
    {
        _animator.SetTrigger("Uber");
    }
}