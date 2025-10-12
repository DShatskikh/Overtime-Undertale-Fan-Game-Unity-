using UnityEngine;

public sealed class Sparkles : MonoBehaviour
{
    [SerializeField]
    private Sprite _deactivatedSprite;

    public void Deactivate()
    {
        GetComponent<SpriteRenderer>().sprite = _deactivatedSprite;
        GetComponent<Collider2D>().enabled = false;
    }
}