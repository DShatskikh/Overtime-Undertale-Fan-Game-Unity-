using System.Collections;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public Vector2 SizeArena = new Vector2(1.15f, 1.15f);
    public abstract IEnumerator AwaitExecute();
}