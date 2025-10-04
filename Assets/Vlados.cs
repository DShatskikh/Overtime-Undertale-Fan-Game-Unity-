using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Vlados : MonoBehaviour
{
    [SerializeField]
    private Text _mainText;
        
    [SerializeField]
    private Text[] _rotateTexts;
        
    private void Start()
    {
        StartCoroutine(AwaitChangeColorAnimation());
        StartCoroutine(AwaitScaleAnimation());
        
        foreach (var rotateText in _rotateTexts)
        {
            StartCoroutine(AwaitRotateAnimation(rotateText));
            StartCoroutine(AwaitRandomMoveAnimation(rotateText));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Application.Quit();
    }

    private IEnumerator AwaitChangeColorAnimation()
    {
        while (true)
        {
            var random = Random.Range(0, 7);
            var targetColor = random switch
            {
                0 => Color.blue,
                1 => Color.green,
                2 => Color.cyan,
                3 => Color.magenta,
                4 => Color.red,
                5 => Color.yellow,
                6 => Color.green,
                _ => throw new ArgumentOutOfRangeException()
            };

            var delta = 0f;
            var startColor = _mainText.color;
            
            while (delta < 1)
            {
                _mainText.color = Color.Lerp(startColor, targetColor, delta);
                delta += Time.deltaTime / 5;
                yield return null;
            }
        }
    }

    private IEnumerator AwaitScaleAnimation()
    {
        while (true)
        {
            var delta = 0f;
            
            while (delta < 1)
            {
                _mainText.transform.localScale = Vector3.Lerp(Vector3.one * 0.9f, Vector3.one * 1.1f, delta);
                delta += Time.deltaTime / 1;
                yield return null;
            }
        
            delta = 0f;
            
            while (delta < 1)
            {
                _mainText.transform.localScale = Vector3.Lerp(Vector3.one * 1.1f, Vector3.one * 0.9f, delta);
                delta += Time.deltaTime / 1;
                yield return null;
            }
        }
    }

    private IEnumerator AwaitRotateAnimation(Text text)
    {
        var direction = Random.Range(0, 50) % 2 == 0 ? 1 : -1;
        text.transform.eulerAngles += new Vector3(0, 0, Random.Range(0, 180));
        var speed = Random.Range(75, 100);
        
        while (true)
        {
            text.transform.eulerAngles += new Vector3(0, 0, Time.deltaTime * direction * speed);
            yield return null;
        }
    }

    private IEnumerator AwaitRandomMoveAnimation(Text text)
    {
        var direction = Random.Range(0, 50) > 25 ? 1 : -1;
        var startPosition = text.transform.position;

        while (true)
        {
            var targetPosition = startPosition + new Vector3(Random.Range(-50f, 50f), Random.Range(-20f, 20f));
            
            while (Vector3.Distance(text.transform.position, targetPosition) < 5)
            {
                text.transform.position = Vector3.MoveTowards(text.transform.position, targetPosition, Time.deltaTime * 50);
                yield return null;
            }
            
            yield return null;
        }
    }
}