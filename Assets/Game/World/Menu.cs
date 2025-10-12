using System;
using UnityEngine;

public sealed class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject _itemPanel;
    
    [SerializeField]
    private GameObject _nodePanel;

    [SerializeField]
    private Replica[] _ceilReplicas;
    
    private Soul _soul;
    private bool _isMainMenuSelected = true;
    private int _mainMenuSelected;
    private bool _isItemMenuSelected;
    private int _itemMenuSelected;
    private bool _isNodeMenuSelected;
    private bool _nodeMenuSelected;

    private void Start()
    {
        _soul = Instantiate(Resources.Load<Soul>("Soul"), transform);
        _soul.transform.localPosition = new Vector3(-3.04f, 1.492f);
        _soul.enabled = false;
    }
    
    private void OnDestroy()
    {
        //Destroy(_soul.gameObject);
    }

    private void Update()
    {
        if (_isMainMenuSelected)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _mainMenuSelected--;
                
                if (_mainMenuSelected < 0)
                    _mainMenuSelected = 0;
            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _mainMenuSelected++;

                if (_mainMenuSelected > 2)
                    _mainMenuSelected = 2;
            }

            _soul.transform.localPosition = _mainMenuSelected switch
            {
                0 => new Vector3(-3.04f, 1.492f),
                1 => new Vector3(-3.04f, 0.51f),
                2 => new Vector3(-3.04f, -0.48f),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        if (_isItemMenuSelected)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _itemMenuSelected--;
                
                if (_itemMenuSelected < 0)
                    _itemMenuSelected = 0;
            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _itemMenuSelected++;

                if (_itemMenuSelected > 7)
                    _itemMenuSelected = 7;
            }
            
            _soul.transform.localPosition = _itemMenuSelected switch
            {
                0 => new Vector3(1.107f, 4.369f),
                1 => new Vector3(1.107f, 4.369f - 0.86f),
                2 => new Vector3(1.107f, 4.369f - 0.86f * 2),
                3 => new Vector3(1.107f, 4.369f - 0.86f * 3),
                4 => new Vector3(1.107f, 4.369f - 0.86f * 4),
                5 => new Vector3(1.107f, 4.369f - 0.86f * 5),
                6 => new Vector3(1.107f, 4.369f - 0.86f * 6),
                7 => new Vector3(1.107f, -1.646f),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isMainMenuSelected)
            {
                switch (_mainMenuSelected)
                {
                    case 0:
                        _isItemMenuSelected = true;
                        _itemPanel.gameObject.SetActive(true);
                        _isMainMenuSelected = false;
                        break;
                    case 1:
                        _isNodeMenuSelected = true;
                        _nodePanel.gameObject.SetActive(true);
                        _isMainMenuSelected = false;
                        _soul.gameObject.SetActive(false);
                        break;
                    case 2:
                        enabled = false;
                        
                        DialogueWindow.Open(_ceilReplicas, () =>
                        {
                            enabled = true;  
                        });
                        break;
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (_isItemMenuSelected)
            {
                _isItemMenuSelected = false;
                _itemPanel.gameObject.SetActive(false);
            }
            
            if (_isNodeMenuSelected)
            {
                _isNodeMenuSelected = false;
                _nodePanel.gameObject.SetActive(false);
                _soul.gameObject.SetActive(true);
            }
            
            _isMainMenuSelected = true;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Destroy(gameObject);
            Player.Instance.enabled = true;
        }
    }
}