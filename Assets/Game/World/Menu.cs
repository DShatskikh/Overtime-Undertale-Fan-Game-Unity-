using System;
using UnityEngine;

public sealed class Menu : MonoBehaviour
{
    [Header("Node")]
    [SerializeField]
    private GameObject _nodePanel;
    
    [SerializeField]
    private TextMesh _nameLabel;
    
    [SerializeField]
    private TextMesh _lvLabel;
    
    [SerializeField]
    private TextMesh _hpLabel;
    
    [SerializeField]
    private TextMesh _killedLabel;

    [SerializeField]
    private TextMesh _hiredLabel;

    [SerializeField]
    private TextMesh _australiumLabel;

    [SerializeField]
    private TextMesh _timeLabel;

    [SerializeField]
    private GameObject _itemPanel;

    [SerializeField]
    private AudioSource _selectSFX;
    
    [SerializeField]
    private AudioSource _menuSelectSFX;

    [SerializeField]
    private TextMesh[] _itemLabels;
    
    [SerializeField]
    private Replica[] _ceilReplicas;

    private Soul _soul;
    private bool _isMainMenuSelected = true;
    private int _mainMenuSelected;
    private bool _isItemMenuSelected;
    private int _itemMenuSelected;
    private bool _isItemDownMenuSelected;
    private int _itemDownMenuSelected;
    private bool _isNodeMenuSelected;
    private bool _nodeMenuSelected;

    private void Start()
    {
        _soul = Instantiate(Resources.Load<Soul>("Soul"), transform);
        _soul.transform.localPosition = new Vector3(-3.04f + 0.592f, 1.492f);
        _soul.enabled = false;

        for (int i = 0; i < _itemLabels.Length; i++)
        {
            _itemLabels[i].text = SaveSystem.GetString($"Item_{i}");
        }

        _nameLabel.text = "\"Denis\"";
        _lvLabel.text = $"LV {PlayerStats.Instance.LV}";
        _hpLabel.text = $"HP {PlayerStats.Instance.HP}/{PlayerStats.Instance.MaxHP}";
        _killedLabel.text = $"KILLED {PlayerStats.Instance.KILLED}";
        _hiredLabel.text = $"HIRED {PlayerStats.Instance.HIRED}";
        _australiumLabel.text = $"AUSTRALIUM {PlayerStats.Instance.AUSTRALIUM}";
        
        var time = GameTimer.Instance.GetTime;
        _timeLabel.text = $"{(int)time - (int)time % 60}:{(int)time % 60}\n";
    }

    private void Update()
    {
        if (_isMainMenuSelected)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _mainMenuSelected--;

                if (_mainMenuSelected < 0)
                {
                    _mainMenuSelected = 0;
                }
                else
                {
                    _selectSFX.Play();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _mainMenuSelected++;

                if (_mainMenuSelected > 2)
                {
                    _mainMenuSelected = 2;
                }
                else
                {
                    _selectSFX.Play();
                }
            }

            _soul.transform.localPosition = _mainMenuSelected switch
            {
                0 => new Vector3(-3.04f + 0.592f, 1.492f),
                1 => new Vector3(-3.04f + 0.592f, 0.51f),
                2 => new Vector3(-3.04f + 0.592f, -0.48f),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        if (_isNodeMenuSelected)
        {
            var time = GameTimer.Instance.GetTime;
            _timeLabel.text = $"{(int)time - (int)time % 60}:{(int)time % 60}\n";
        }
        
        if (_isItemMenuSelected)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _itemMenuSelected--;

                if (_itemMenuSelected < 0)
                {
                    _itemMenuSelected = 0;
                }
                else
                {
                    _selectSFX.Play();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _itemMenuSelected++;

                if (_itemMenuSelected > 7)
                {
                    _itemMenuSelected = 7;
                }
                else
                {
                    _selectSFX.Play();
                }
            }
            
            _soul.transform.localPosition = _itemMenuSelected switch
            {
                0 => new Vector3(2.4f, 4.369f),
                1 => new Vector3(2.4f, 4.369f - 0.86f),
                2 => new Vector3(2.4f, 4.369f - 0.86f * 2),
                3 => new Vector3(2.4f, 4.369f - 0.86f * 3),
                4 => new Vector3(2.4f, 4.369f - 0.86f * 4),
                5 => new Vector3(2.4f, 4.369f - 0.86f * 5),
                6 => new Vector3(2.4f, 4.369f - 0.86f * 6),
                7 => new Vector3(2.4f, -1.646f),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        if (_isItemDownMenuSelected)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _itemDownMenuSelected++;

                if (_itemDownMenuSelected > 2)
                {
                    _itemDownMenuSelected = 2;
                }
                else
                {
                    _selectSFX.Play();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _itemDownMenuSelected--;

                if (_itemDownMenuSelected < 0)
                {
                    _itemDownMenuSelected = 0;
                }
                else
                {
                    _selectSFX.Play();
                }
            }
            
            _soul.transform.localPosition = _itemDownMenuSelected switch
            {
                0 => new Vector3(2.4f, -3.218f),
                1 => new Vector3(5f + 0.592f, -3.218f),
                2 => new Vector3(8.2f + 0.592f, -3.218f),
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
                        _menuSelectSFX.Play();
                        break;
                    case 1:
                        _isNodeMenuSelected = true;
                        _nodePanel.gameObject.SetActive(true);
                        _isMainMenuSelected = false;
                        _soul.gameObject.SetActive(false);
                        _menuSelectSFX.Play();
                        break;
                    case 2:
                        enabled = false;
                        _menuSelectSFX.Play();
                        
                        DialogueWindow.Open(_ceilReplicas, () =>
                        {
                            enabled = true;  
                        }, false);
                        break;
                }
            }
            else if (_isItemMenuSelected)
            {
                var itemName = SaveSystem.GetString($"Item_{_itemMenuSelected}");

                if (itemName != string.Empty)
                {
                    _isItemMenuSelected = false;
                    _isItemDownMenuSelected = true;
                    
                    _selectSFX.Play();
                }
                else
                {
                    Debug.Log("Предмет отсутствует");
                }
                    
                return;
            }
            else if (_isItemDownMenuSelected)
            {
                var itemName = SaveSystem.GetString($"Item_{_itemMenuSelected}");

                if (_itemDownMenuSelected == 0)
                {
                    Debug.Log("Использован предмет " + itemName);
                    
                    _soul.transform.position = transform.position + new Vector3(-3.03999996f + 0.592f,1.49199998f);
                    _isItemDownMenuSelected = false;
                    _itemPanel.SetActive(false);
                    enabled = false;
                    
                    _itemLabels[_itemMenuSelected].text = string.Empty;
                    SaveSystem.SetString($"Item_{_itemMenuSelected}", string.Empty);
                    _selectSFX.Play();
                    
                    DialogueWindow.Open(_ceilReplicas, () =>
                    {
                        enabled = true;
                        _isMainMenuSelected = true;
                    }, false);
                }
                else if (_itemDownMenuSelected == 1)
                {
                    Debug.Log("Инфо " + itemName);
                }
                else
                {
                    Debug.Log("Предмет выкинут " + itemName);
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (_isItemMenuSelected)
            {
                _isItemMenuSelected = false;
                _itemPanel.gameObject.SetActive(false);
                _isMainMenuSelected = true;
            }
            
            if (_isNodeMenuSelected)
            {
                _isNodeMenuSelected = false;
                _nodePanel.gameObject.SetActive(false);
                _soul.gameObject.SetActive(true);
                _isMainMenuSelected = true;
            }

            if (_isItemDownMenuSelected)
            {
                _isItemMenuSelected = true;
                _isItemDownMenuSelected = false;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Destroy(gameObject);
            Player.Instance.enabled = true;
        }
    }
}