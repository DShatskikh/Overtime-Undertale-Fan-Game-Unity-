using UnityEngine;

public sealed class OutdoorSurveillanceCamera : MonoBehaviour
{
    [SerializeField]
    private Sprite _left;
    
    [SerializeField]
    private Sprite _center;
    
    [SerializeField]
    private Sprite _right;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (transform.position.x - Player.Instance.transform.position.x > 1.5f)
        {
            _spriteRenderer.sprite = _left;
        }
        else if (transform.position.x - Player.Instance.transform.position.x < -1.5f)
        {
            _spriteRenderer.sprite = _right;
        }
        else
        {
            _spriteRenderer.sprite = _center;
        }
    }
}