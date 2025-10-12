using System;
using UnityEngine;

public sealed class Puzzle_1 : MonoBehaviour
{
    [SerializeField]
    private Sparkles[] _sparkles;

    [SerializeField]
    private Sprite _activatedSprite;
    
    [SerializeField]
    private AudioSource _activateSFX;
    
    private bool _isActivated;

    private void Start()
    {
        if (SaveSystem.GetBool("IsPuzzle_1_Activated"))
        {
            _isActivated = true;
            GetComponent<SpriteRenderer>().sprite = _activatedSprite;
            
            foreach (var sparkle in _sparkles)
            {
                sparkle.Deactivate();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isActivated)
            return;
        
        if (other.GetComponent<Player>())
        {
            _isActivated = true;
            GetComponent<SpriteRenderer>().sprite = _activatedSprite;
            _activateSFX.Play();
            
            foreach (var sparkle in _sparkles)
            {
                sparkle.Deactivate();
            }
            
            SaveSystem.SetBool("IsPuzzle_1_Activated", true);
        }
    }
}