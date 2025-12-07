using UnityEngine;

public sealed class FlowerPair : MonoBehaviour, IUsable
{
    [SerializeField]
    private Replica[] _replicas_1, _replicas_2;

    [SerializeField]
    private FlowerPair _flower;

    public bool IsSecond = false;

    public void Use()
    {
        Player.Instance.enabled = false;
        
        if (!IsSecond)
        {
            _flower.IsSecond = true;
            DialogueWindow.Open(_replicas_1, () =>
            {
                Player.Instance.enabled = true;
            });
        }
        else
        {
            DialogueWindow.Open(_replicas_2, () =>
            {
                Player.Instance.enabled = true;
            });
        }
    }
}