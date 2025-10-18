using System;
using UnityEngine;

public sealed class Sparkles : MonoBehaviour
{
    [SerializeField]
    private Sprite _deactivatedSprite;
    
    private Sprite _activatedSprite;

    private void Awake()
    {
        _activatedSprite = GetComponent<SpriteRenderer>().sprite;
    }

    public void Activate()
    {
        GetComponent<SpriteRenderer>().sprite = _activatedSprite;
        GetComponent<Collider2D>().enabled = true;
    }
    
    public void Deactivate()
    {
        GetComponent<SpriteRenderer>().sprite = _deactivatedSprite;
        GetComponent<Collider2D>().enabled = false;
    }
}