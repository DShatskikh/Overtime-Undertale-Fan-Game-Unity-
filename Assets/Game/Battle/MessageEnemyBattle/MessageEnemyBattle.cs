using System;
using System.Collections;
using UnityEngine;

public sealed class MessageEnemyBattle : MonoBehaviour
{
    [SerializeField]
    private TextMesh _label;
    
    private AudioSource _sfx;
    private Action _endAction;
    private bool _isSkip;
    private int _currentReplicaIndex;
    private string[] _replicas;

    public static AudioClip SFX;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isSkip = true;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isSkip = true;
        }
    }

    public void Open(string[] replicas, Action endAction)
    {
        _endAction = endAction;
        _sfx = GetComponent<AudioSource>();

        if (SFX)
        {
            _sfx.clip = SFX;
        }
        
        StartCoroutine(AwaitWrite(replicas));
    }

    private IEnumerator AwaitWrite(string[] replicas)
    {
        _replicas = replicas;
        var replica = replicas[_currentReplicaIndex];
        var text = string.Empty;

        for (int i = 0; i < replica.Length; i++)
        {
            text += replica[i];
            _label.text = text;

            if (!_isSkip)
            {
                _sfx.Play();
                yield return new WaitForSeconds(0.1f);
            }
        }
            
        yield return null;
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        _isSkip = false;
        _currentReplicaIndex++;

        if (_currentReplicaIndex == _replicas.Length)
        {
            _endAction.Invoke();
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(AwaitWrite(replicas));
        }
    }
}