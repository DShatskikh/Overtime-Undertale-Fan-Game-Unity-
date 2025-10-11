using UnityEngine;

public sealed class TransitionTrigger : MonoBehaviour
{
    [SerializeField]
    private int _transitionSceneIndex = 4;
    
    [SerializeField]
    private int _transitionPointIndex = -1;
    
    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            TransitionScreen.Transition(_transitionSceneIndex, _transitionPointIndex);
        }
    }
}