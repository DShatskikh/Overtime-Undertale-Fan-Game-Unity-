using System;
using UnityEngine;

public sealed class Player : MonoBehaviour
{
    private const float SPEED = 3;
    private const float RUN_SPEED = 5;
    
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Vector2 _previousPosition;
    public static Player Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        Stop();
    }

    private void Update()
    {
        var isRun = Input.GetKey(KeyCode.LeftShift);
        var moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveDirection.magnitude != 0)
        {
            Move(moveDirection, isRun);
        }
        else
        {
            Stop();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SearchUsableAndUse();
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Instantiate(Resources.Load<Menu>("Menu"),  
                new Vector3(Camera.main.transform.position.x -3.02f,
                    Camera.main.transform.position.y + 0.4300001f), Quaternion.identity, transform);
            enabled = false;
        }
    }   

    private void FixedUpdate()
    {
        _previousPosition = _rigidbody.position;
    }

    private void SearchUsableAndUse()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
    
        if (colliders.Length > 0)
        {
            var minDistance = float.MaxValue;
            IUsable nearestUseObject = null;
                
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IUsable useObject))
                {
                    var currentDistance = Vector2.Distance(transform.position, ((MonoBehaviour)useObject).transform.position);
                        
                    if (minDistance > currentDistance)
                    {
                        minDistance = currentDistance;
                        nearestUseObject = useObject;
                    }
                }
            }
                
            nearestUseObject?.Use();
        }
    }
    
    private void Move(Vector2 direction, bool isRun)
    {
        if (direction.x == 0)
        {
            _animator.SetFloat("Vertical", direction.y);
            _animator.SetFloat("Horizontal", 0);
        }
        
        if (direction.y == 0)
        {
            _animator.SetFloat("Vertical", 0);
            _animator.SetFloat("Horizontal", direction.x);
        }

        if ((_previousPosition - _rigidbody.position).magnitude > 0)
            _animator.SetFloat("Speed", !isRun ? 0.5f : 1);
        else
            _animator.SetFloat("Speed", 0);

        _rigidbody.linearVelocity = direction * (isRun ? RUN_SPEED : SPEED);
    }

    private void Stop()
    {
        _rigidbody.linearVelocity = Vector2.zero;
        _animator.SetFloat("Speed", 0);
    }
}