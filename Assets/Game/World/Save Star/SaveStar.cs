using UnityEngine;

public sealed class SaveStar : MonoBehaviour, IUsable
{
    [SerializeField]
    private string _locationName = "???";
    
    [SerializeField]
    private Replica[] _replicas;

    public void Use()
    {
        Player.Instance.enabled = false;
        
        DialogueWindow.Open(_replicas, () =>
        {
            Player.Instance.enabled = false;
            Instantiate(Resources.Load<SaveScreen>("Save Screen"), 
                new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 1), Quaternion.identity)
                .Init(_locationName);
        });
    }
}