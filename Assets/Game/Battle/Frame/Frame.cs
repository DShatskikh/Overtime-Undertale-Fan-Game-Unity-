using System;
using System.Collections;
using UnityEngine;

public sealed class Frame : MonoBehaviour
{
    private const float SPEED = 5;

    [SerializeField]
    private Transform[] _lines = new Transform[4];
    
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Up
        _lines[0].localScale = new Vector3(_spriteRenderer.size.x, 0.05f);
        _lines[0].localPosition = new Vector3(0,  _spriteRenderer.size.y / 2);
        
        // Down
        _lines[1].localScale = new Vector3(_spriteRenderer.size.x, 0.05f);
        _lines[1].localPosition = new Vector3(0, -_spriteRenderer.size.y / 2);
        
        // Right
        _lines[2].localScale = new Vector3(0.05f, _spriteRenderer.size.y);
        _lines[2].localPosition = new Vector3(_spriteRenderer.size.x / 2, 0);
        
        // Left
        _lines[3].localScale = new Vector3(0.05f, _spriteRenderer.size.y);
        _lines[3].localPosition = new Vector3(-_spriteRenderer.size.x / 2, 0);
    }

    public void SetSize(float width, float height)
    {
        StartCoroutine(AwaitUpgradeSize(width, height));
    }

    public IEnumerator AwaitUpgradeSize(float width, float height)
    {
        while (Math.Abs(_spriteRenderer.size.x - width) > 0.1f || Math.Abs(_spriteRenderer.size.y - height) > 0.1f)
        {
            if (_spriteRenderer.size.x > width)
                _spriteRenderer.size -= new Vector2(Time.deltaTime * SPEED, 0);
            
            if (_spriteRenderer.size.x < width)
                _spriteRenderer.size += new Vector2(Time.deltaTime * SPEED, 0);
            
            if (_spriteRenderer.size.y > height)
                _spriteRenderer.size -= new Vector2(0, Time.deltaTime * SPEED);
            
            if (_spriteRenderer.size.y > height)
                _spriteRenderer.size += new Vector2(0, Time.deltaTime * SPEED);
            
            yield return null;
        }
        
        _spriteRenderer.size = new Vector2(width, height);
    }
}