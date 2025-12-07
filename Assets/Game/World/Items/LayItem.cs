using UnityEngine;

public sealed class LayItem : MonoBehaviour, IUsable
{
    [SerializeField]
    private string _id;

    [SerializeField]
    private string _saveId;
    
    [SerializeField]
    private Replica[] _replicas;

    private void Start()
    {
        if (SaveSystem.GetBool($"PickUp_{_saveId}"))
        {
            Destroy(gameObject);
        }
    }

    public void Use()
    {
        Player.Instance.enabled = false;

        for (int i = 0; i < 8; i++)
        {
            if (SaveSystem.GetString($"Item_{i}") == string.Empty)
            {
                Destroy(gameObject);
                Debug.Log("Подобрали предмет");
                SaveSystem.SetString($"Item_{i}", _id);
                SaveSystem.SetBool($"PickUp_{_saveId}", true);

                DialogueWindow.Open(_replicas, () =>
                {
                    Player.Instance.enabled = true;
                }); 
                
                return;   
            }
        }

        Player.Instance.enabled = false;
        Debug.Log("Не подобрали предмет, инвентарь забит");
    }
}