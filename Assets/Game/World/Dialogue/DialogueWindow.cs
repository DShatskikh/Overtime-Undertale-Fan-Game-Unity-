using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class DialogueWindow : MonoBehaviour
{
    [SerializeField]
    private DialogueLine _dialogueLinePrefab;

    [SerializeField]
    private Image _icon;
    
    [SerializeField]
    private RectTransform _container;

    [SerializeField]
    private RectTransform _iconContainer;
    
    [SerializeField]
    private RectTransform _frame;
    
    [SerializeField]
    private Animator _animator;
    
    [SerializeField]
    private AudioSource _sfx;

    private Action _endAction;
    private List<DialogueLine> _lines = new();
    private static DialogueWindow _instance;
    private Replica[] _replicas;

    private int _currentReplicaIndex;
    private int _currentDialogueIndex;
    private int _currentLetterIndex;
    private bool _isSkip;

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

    public static void Open(Replica[] replicas, Action endAction, bool isPosition = true)
    {
        var prefab = Resources.Load<DialogueWindow>("Dialogue Window");
        _instance = Instantiate(prefab);

        _instance._endAction = endAction;

        if (isPosition)
        {
            if (Player.Instance.transform.position.y + 2f < Camera.main.transform.position.y)
                _instance._frame.anchoredPosition = new Vector2(_instance._frame.anchoredPosition.x, 506.2f); 
        }

        _instance.StartCoroutine(_instance.AwaitWrite(replicas));
    }

    private IEnumerator AwaitWrite(Replica[] replicas)
    {
        _replicas = replicas;

        if (replicas[_currentReplicaIndex].Icon != null)
        {
            _animator.enabled = false;
            _icon.sprite = replicas[_currentReplicaIndex].Icon; 
            _iconContainer.gameObject.SetActive(true);
        }
        else if (replicas[_currentReplicaIndex].AnimationName != string.Empty)
        {
            _animator.enabled = true;
            _animator.CrossFade(replicas[_currentReplicaIndex].AnimationName, 0);
            _iconContainer.gameObject.SetActive(true);
        }
        else
        {
            _iconContainer.gameObject.SetActive(false);
        }

        foreach (var d in replicas[_currentReplicaIndex].Dialogues)
        {
            var dialogue = d.Replace("&", "Denis");
            var line = Instantiate(_dialogueLinePrefab, _container);
            var text = string.Empty;
            _lines.Add(line);
            
            if (replicas[_currentReplicaIndex].SFX)
                _sfx.clip = replicas[_currentReplicaIndex].SFX;
            
            for (int i = 0; i < dialogue.Length; i++)
            {
                _currentLetterIndex = i;
                text += dialogue[i];
                line.SetText(text);

                if (!_isSkip)
                {
                    _sfx.Play();
                    yield return new WaitForSeconds(0.1f); // replicas[_currentReplicaIndex].Speed
                }
            }
            
            _currentDialogueIndex++;
        }

        if (replicas[_currentReplicaIndex].AnimationName != string.Empty)
            _animator.SetTrigger("StopSpeak");
        
        yield return null;
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        _isSkip = false;
        _currentReplicaIndex++;
        
        foreach (var line in _lines)
        {
            Destroy(line.gameObject);
        }

        _lines = new List<DialogueLine>();
        
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