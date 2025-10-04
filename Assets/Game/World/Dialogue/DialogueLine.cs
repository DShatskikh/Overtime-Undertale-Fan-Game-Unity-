using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class DialogueLine : MonoBehaviour
{
    [SerializeField]
    private Text _line;

    [SerializeField]
    private RectTransform _rectTransform;
    
    public string GetText => _line.text;

    public void SetText(string text)
    {
        _line.text = text;
        var textHeight = _line.preferredHeight;

        _line.GetComponent<RectTransform>().sizeDelta = new Vector2(_line.GetComponent<RectTransform>().sizeDelta.x, textHeight);
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, textHeight);
    }
}