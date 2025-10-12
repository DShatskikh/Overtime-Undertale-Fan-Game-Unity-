using UnityEngine;

public sealed class GameTimer : MonoBehaviour
{
    private float _time;
    public float GetTime => _time;
    public static GameTimer Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        _time = SaveSystem.GetFloat("Time");
    }

    private void Update()
    {
        _time += Time.deltaTime;
    }
}