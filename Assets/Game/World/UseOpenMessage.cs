using UnityEngine;

public sealed class UseOpenMessage : MonoBehaviour, IUsable
{
    [SerializeField]
    private Replica[] _replicas;

    public void Use()
    {
        Player.Instance.enabled = false;
        
        DialogueWindow.Open(_replicas, () =>
        {
            Player.Instance.enabled = true;
        });
    }
}