using UnityEngine;

public sealed class Puzzle_2 : MonoBehaviour
{
    [SerializeField]
    private PlateNumber[] _plates;

    [SerializeField]
    private Puzzle_2_Lever _lever;

    [SerializeField]
    private Sparkles[] _sparkles;
    
    [SerializeField]
    private string _correctCode;
    
    private string _code = string.Empty;
    public bool CanStep => _code.Length < 3;
    public bool IsCorrectCode => _code == _correctCode;
    public bool IsDecied { get; private set; }

    private void Start()
    {
        _lever.Init(this);
        
        for (var i = 0; i < _plates.Length; i++)
        {
            var plate = _plates[i];
            plate.Init(this, i + 1);
        }

        if (SaveSystem.GetBool("IsPuzzle_2_Activated"))
        {
            Decied();
        }
    }

    public void Step(int number)
    {
        _code += number;
    }

    public void Reset()
    {
        _code = string.Empty;
        
        foreach (var plate in _plates)
        {
            plate.Reset();
        }
    }

    public void Decied()
    {
        foreach (var sparkle in _sparkles)
        {
            sparkle.Deactivate();
        }

        foreach (var plate in _plates)
        {
            plate.Reset();
        }

        _code = _correctCode;
        _lever.Activate();
        IsDecied = true;
        SaveSystem.SetBool("IsPuzzle_2_Activated", true);
    }
}