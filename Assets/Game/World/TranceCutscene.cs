using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class TranceCutscene : MonoBehaviour
{
    [SerializeField]
    private Replica[] _replicas;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player.Instance.enabled = false;
        DialogueWindow.Open(_replicas, () =>
        {
            SceneManager.LoadScene(3);
        });
    }
}