using UnityEngine;

public sealed class SelectLine : MonoBehaviour
{
    [SerializeField]
    private TextMesh _label;

    [SerializeField]
    private TextMesh _starLabel;
    
    public void Init(string enemyName, bool isYellow = false)
    {
        _label.text = enemyName;

        if (isYellow)
        {
            _label.color = Color.yellow;  
            _starLabel.color = Color.yellow;  
        }
    }
}