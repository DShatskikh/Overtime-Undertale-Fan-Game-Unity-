using UnityEngine;

public sealed class BattleButton : MonoBehaviour
{
    [SerializeField]
    private Sprite _activateSprite;
    
    [SerializeField]
    private Sprite _deactivateSprite;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Activate()
    {
        _spriteRenderer.sprite = _activateSprite;
    }

    public void Deactivate()
    {
        _spriteRenderer.sprite = _deactivateSprite;
    }
}