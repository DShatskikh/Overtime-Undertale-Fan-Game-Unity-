using UnityEngine;

public sealed class TransitionPoint : MonoBehaviour
{
    [SerializeField]
    private int _index;

    public int GetIndex => _index;
}