using System.Collections;
using UnityEngine;

public sealed class Puzzle_2_Lever : MonoBehaviour, IUsable
{
    [SerializeField]
    private Sprite _activate, _deactivate;

    [SerializeField]
    private AudioSource _sfx;
    
    private SpriteRenderer _spriteRenderer;
    private Puzzle_2 _manager;

    public void Init(Puzzle_2 manager)
    {
        _manager = manager;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Activate()
    {
        _spriteRenderer.sprite = _activate;
    }
    
    public void Use()
    {
        if (_manager.IsDecied)
            return;

        _sfx.Play();
        //_spriteRenderer.sprite = _activate;

        StartCoroutine(AwaitReset()); 
    }

    private IEnumerator AwaitReset()
    {
        var blackScreen = Instantiate(Resources.Load<BlackScreenAnimation>("Black Screen Animation"));
        yield return blackScreen.AwaitShowAnimation();
        yield return new WaitForSeconds(0.1f);
        
        if (!_manager.IsCorrectCode)
            _manager.Reset();
        
        yield return blackScreen.AwaitHideAnimation();
    }
}