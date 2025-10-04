using UnityEngine;
using UnityEngine.Tilemaps;

public sealed class Barriers : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TilemapRenderer>().enabled = false;
    }
}