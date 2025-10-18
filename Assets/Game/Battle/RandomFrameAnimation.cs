using UnityEngine;
using Random = UnityEngine.Random;

public sealed class RandomFrameAnimation : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Animator>().Play(GetComponent<Animator>()
            .GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, Random.Range(0, 1f));
    }
}