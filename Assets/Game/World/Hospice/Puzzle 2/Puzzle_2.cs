using UnityEngine;

public sealed class Puzzle_2 : MonoBehaviour
{
    [SerializeField]
    private string _id;
    
    [SerializeField]
    private string _correctCode;

    [SerializeField]
    private Puzzle_2_Lever _lever;

    [SerializeField]
    private PlateNumber[] _plates;

    [SerializeField]
    private Sparkles[] _sparkles;

    private string _code = string.Empty;
    public bool CanStep => _code.Length < IsCorrectCode.ToString().Length;
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

        if (SaveSystem.GetBool($"IsPuzzle_{_id}_Activated"))
        {
            Decied();
        }
    }

    public void Step(int number)
    {
        _code += number;

        if (_code == _correctCode)
            Decied();
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
            plate.Activate();
        }

        _code = _correctCode;
        _lever.Activate();
        IsDecied = true;
        SaveSystem.SetBool("IsPuzzle_2_Activated", true);
    }
}