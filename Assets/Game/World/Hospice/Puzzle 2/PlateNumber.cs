using UnityEngine;

public sealed class PlateNumber : MonoBehaviour
{
    [SerializeField]
    private TextMesh _label;

    [SerializeField]
    private Sprite _normalSprite, _activateSprite;

    private SpriteRenderer _spriteRenderer;
    private Puzzle_2 _manager;
    private float _startY;
    private int _number;

    public void Init(Puzzle_2 manager, int number)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _startY = _label.transform.localPosition.y;
        _number = number;
        
        _manager = manager;
        _label.text = number.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Player player))
            return;

        if (!_manager.CanStep)
            return;

        _manager.Step(_number);
        GetComponent<AudioSource>().Play();
        Activate();
    }

    public void Activate()
    {
        _spriteRenderer.sprite = _activateSprite;
        _label.color = Color.yellow;
        _label.transform.localPosition = new Vector3(0, 0.369f);
    }
    
    public void Reset()
    {
        _spriteRenderer.sprite = _normalSprite;
        _label.color = new Color(58 / 255f, 58 / 255f, 58 / 255f);
        _label.transform.localPosition = new Vector3(0, _startY);
    }
}