using System;
using System.Collections;
using UnityEngine;

public sealed class Frame : MonoBehaviour
{
    private const float SPEED = 5;
    
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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