using UnityEngine;

public sealed class ItemLine : MonoBehaviour
{
    [SerializeField]
    private TextMesh _label;

    public void Init(string enemyName)
    {
        _label.text = enemyName;
    }
}