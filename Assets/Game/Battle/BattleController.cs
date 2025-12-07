using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; private set; }

    [SerializeField]
    private Enemy[] _enemies;

    [SerializeField]
    private BattleButton[] _battleButtons;

    [SerializeField]
    private AudioSource _sfxSelect;
    
    [SerializeField]
    private AudioSource _sfxMenuSelect;
    
    [SerializeField]
    private TextMesh _line;

    [SerializeField]
    private GameObject _fightSelect;
    
    [SerializeField]
    private GameObject _fightIndicator;
    
    [SerializeField]
    private GameObject _actSelect;
    
    [SerializeField]
    private GameObject _act;

    [SerializeField]
    private GameObject _itemSelect;
    
    [SerializeField]
    private GameObject _mercySelect;

    [SerializeField]
    private TextMesh _itemPageLabel;

    [SerializeField]
    private Transform _indicatorStick;
    
    [SerializeField]
    private Animator _gunAnimator;

    [SerializeField]
    private AudioSource _shotSFX;

    [SerializeField]
    private AudioSource _damageSFX;
    
    [SerializeField]
    private AudioSource _writeSFX;

    [SerializeField]
    private AudioSource _spareSFX;
    
    [SerializeField]
    private Frame _frame;

    private bool _isTurnPlayer = true;
    private bool _isMainMenu = true;
    private Coroutine _writeCoroutine;
    private int _indexMenuButton;
    private int _indexFightSelect;
    private bool _isFightSelect;
    private List<EnemySelectLine> _fightSelectLines = new();
    private bool _isFightIndicator;
    private int _indexActSelect;
    private bool _isActSelect;
    private List<EnemySelectLine> _actSelectLines = new();
    private int _indexAct;
    private bool _isAct;
    private List<SelectLine> _actLines = new();
    private int _indexItemSelect;
    private bool _isItemSelect;
    private List<ItemLine> _itemSelectLines = new();
    private int _pageNumber = 1;
    private List<SelectLine> _mercySelectLines = new();
    private int _indexMercySelect;
    private bool _isMercySelect;

    public Frame GetFrame => _frame;
    public IReadOnlyCollection<Enemy> GetEnemies => _enemies;
    public string BattleApproachMessage;
    private AudioClip _previousMusic;
    private bool _isRun;
    public bool IsGetWriteProcessing => _writeCoroutine != null;

    private void Awake()
    {
        Instance = this;
        
        if (Player.Instance)
            transform.position = Player.Instance.transform.position;

        if (!Soul.Instance)
        {
            Instantiate(Resources.Load<Soul>("Soul"));
            Soul.Instance.transform.localPosition = new Vector3(0, -2.2f);
            Soul.Instance.enabled = false;
        }
    }

    public void Init(BattleData battleDataData)
    {
        Instantiate(battleDataData.Background, transform.position, Quaternion.identity, transform);
        _enemies = new Enemy[battleDataData.Enemies.Length];

        for (var index = 0; index < battleDataData.Enemies.Length; index++)
        {
            var prefab = battleDataData.Enemies[index];
            var enemy = Instantiate(prefab, transform.position + new Vector3(
                index switch {0 => 0, 1 => -6.34f, 2 => 6.34f}, prefab.transform.position.y), 
                Quaternion.identity, transform);
            enemy.Init(this);
            _enemies[index] = enemy;
        }

        _previousMusic = MusicPlayer.Instance.GetClip;
        MusicPlayer.Instance.Play(battleDataData.Music);
        WriteStartMessage();
    }

    private void Update()
    {
        if (_isTurnPlayer)
        {
            if (_isMainMenu)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    _indexMenuButton++;
                
                    if (_indexMenuButton > _battleButtons.Length - 1)
                        _indexMenuButton = _battleButtons.Length - 1;
                    else
                        _sfxSelect.Play();
                    
                    return;
                }
            
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    _indexMenuButton--;

                    if (_indexMenuButton < 0)
                        _indexMenuButton = 0;
                    else
                        _sfxSelect.Play();
                    
                    return;
                }

                for (int i = 0; i < _battleButtons.Length; i++)
                {
                    if (i == _indexMenuButton)
                    {
                        _battleButtons[i].Activate();
                        Soul.Instance.transform.position = _battleButtons[i].transform.position + new Vector3(-1, 0);
                    }
                    else
                    {
                        _battleButtons[i].Deactivate();
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _isMainMenu = false;
                    StopWrite();
                    
                    for (int i = 0; i < _battleButtons.Length; i++)
                    {
                        _battleButtons[i].Deactivate();
                    }

                    if (_writeCoroutine != null)
                    {
                        StopCoroutine(_writeCoroutine);
                        _writeCoroutine = null;
                    }
                    
                    _line.gameObject.SetActive(false);
                    _sfxMenuSelect.Play();
                    
                    switch (_indexMenuButton)
                    {
                        case 0:
                            _isFightSelect = true;
                            _fightSelect.SetActive(true);

                            for (int i = 0; i < _enemies.Length; i++)
                            {
                                if (_enemies[i].IsSpare || _enemies[i].IsDead)
                                    continue;
                                
                                var _enemySelectLine = Instantiate(Resources.Load<EnemySelectLine>("Enemy Select Line"),
                                     transform.position + new Vector3(-5.49f, i switch { 0 => -0.75f, 1 => -1.46f, 2 => -2.191f }, 0), Quaternion.identity, _fightSelect.transform);
                                _enemySelectLine.Init(_enemies[i].Name, _enemies[i].Health, _enemies[i].MaxHealth, 
                                    _enemies[i].IsYellowName && !_enemies[i].IsSpare && !_enemies[i].IsDead);
                                _fightSelectLines.Add(_enemySelectLine);
                            }
                            break;
                        case 1:
                            _isActSelect = true;
                            _actSelect.SetActive(true);

                            for (int i = 0; i < _enemies.Length; i++)
                            {
                                if (_enemies[i].IsSpare || _enemies[i].IsDead)
                                    continue;
                                
                                var enemySelectLine = Instantiate(Resources.Load<EnemySelectLine>("Enemy Select Line"),
                                    transform.position + new Vector3(-5.49f, 
                                        i switch { 0 => -0.75f, 1 => -1.46f, 2 => -2.191f }, 0), Quaternion.identity, _actSelect.transform);
                                enemySelectLine.Init(_enemies[i].Name, _enemies[i].Health, _enemies[i].MaxHealth, 
                                    _enemies[i].IsYellowName && !_enemies[i].IsSpare && !_enemies[i].IsDead);
                                _actSelectLines.Add(enemySelectLine);
                            }
                            break;
                        case 2:
                            _isItemSelect = true;
                            _itemSelect.SetActive(true);

                            for (int i = 0; i < 4; i++)
                            {
                                var itemLine = Instantiate(Resources.Load<ItemLine>("Item Line"),
                                    transform.position + i switch
                                    {
                                        0 =>  new Vector3(-5.49f, -0.75f),
                                        1 => new Vector3(1f, -0.75f),
                                        2 => new Vector3(-5.49f, -1.56f),
                                        3 => new Vector3(1f, -1.56f),
                                    }, Quaternion.identity, _itemSelect.transform);
                                itemLine.Init(i switch
                                {
                                    0 => SaveSystem.GetString("Item1"),
                                    1 => SaveSystem.GetString("Item2"),
                                    2 => SaveSystem.GetString("Item3"),
                                    3 => SaveSystem.GetString("Item4"),
                                });
                                _itemSelectLines.Add(itemLine);
                            }
                            break;
                        case 3:
                            _isMercySelect = true;
                            _mercySelect.SetActive(true);
                            var hireLine = Instantiate(Resources.Load<SelectLine>("Select Line"),
                                transform.position + new Vector3(-5.49f, -0.75f), Quaternion.identity, _mercySelect.transform);
                            hireLine.Init("Hire", _enemies.Any(enemy => enemy.IsYellowName && !enemy.IsSpare));
                            _mercySelectLines.Add(hireLine);

                            if (_enemies[0].IsRun)
                            {
                                var runLine = Instantiate(Resources.Load<SelectLine>("Select Line"),
                                    transform.position + new Vector3(-5.49f, -1.56f), Quaternion.identity, _mercySelect.transform);
                                runLine.Init("Run", false);
                                _mercySelectLines.Add(runLine);
                            }
                            break;
                    }
                    
                    return;
                } 
            }

            if (_isFightSelect)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    _indexFightSelect -= 1;

                    if (_indexFightSelect < 0)
                        _indexFightSelect = 0;
                }
                
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    _indexFightSelect += 1;
                    
                    if (_indexFightSelect > _enemies.Length - 1)
                        _indexFightSelect = _enemies.Length - 1;
                }
                
                Soul.Instance.transform.position = transform.position + _indexFightSelect switch
                {
                    0 => new Vector3(-6.178f, -1.177f),
                    1 => new Vector3(-6.178f, -1.908f),
                    2 => new Vector3(-6.178f, -2.817f),
                };

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    _isFightSelect = false;
                    _isMainMenu = true;
                    _fightSelect.SetActive(false);
                    WriteStartMessage();
                    _sfxMenuSelect.Play();

                    foreach (var line in _fightSelectLines)
                    {
                        Destroy(line.gameObject);
                    }

                    _fightSelectLines = new List<EnemySelectLine>();
                    return;
                }
                
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (_enemies[_indexFightSelect].IsSpare || _enemies[_indexFightSelect].IsDead)
                        return;
                    
                    _isFightSelect = false;
                    _fightSelect.SetActive(false);
                    _sfxMenuSelect.Play();

                    foreach (var line in _fightSelectLines)
                    {
                        Destroy(line.gameObject);
                    }

                    _fightSelectLines = new List<EnemySelectLine>();
 
                    StartCoroutine(AwaitFightIndicator());
                    return;
                }
            }

            if (_isActSelect)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    _indexActSelect -= 1;

                    if (_indexActSelect < 0)
                        _indexActSelect = 0;
                }
                
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    _indexActSelect += 1;
                    
                    if (_indexActSelect > _enemies.Length - 1)
                        _indexActSelect = _enemies.Length - 1;
                }
                
                Soul.Instance.transform.position = transform.position + _indexActSelect switch
                {
                    0 => new Vector3(-6.178f, -1.177f),
                    1 => new Vector3(-6.178f, -1.908f),
                    2 => new Vector3(-6.178f, -2.817f),
                };
                
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    _isActSelect = false;
                    _isMainMenu = true;
                    _actSelect.SetActive(false);
                    WriteStartMessage();
                    _sfxMenuSelect.Play();

                    foreach (var line in _actSelectLines)
                    {
                        Destroy(line.gameObject);
                    }

                    _actSelectLines = new List<EnemySelectLine>();
                    return;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (_enemies[_indexActSelect].IsSpare || _enemies[_indexActSelect].IsDead)
                        return;
                    
                    _isActSelect = false;
                    _isAct = true;
                    _actSelect.SetActive(false);
                    _act.SetActive(true);
                    _sfxMenuSelect.Play();

                    foreach (var line in _actSelectLines)
                    {
                        Destroy(line.gameObject);
                    }

                    _actSelectLines = new List<EnemySelectLine>();
                    var acts = new List<string> { "Inspection" };
                    acts.AddRange(_enemies[_indexActSelect].Acts);
                    
                    for (int i = 0; i < acts.Count; i++)
                    {
                        var actLine = Instantiate(Resources.Load<SelectLine>("Select Line"),
                            transform.position + i switch
                            {
                                0 =>  new Vector3(-5.49f, -0.75f),
                                1 => new Vector3(1f, -0.75f),
                                2 => new Vector3(-5.49f, -1.56f),
                                3 => new Vector3(1f, -1.56f),
                            }, Quaternion.identity, _act.transform);
                        actLine.Init(acts[i]);
                        _actLines.Add(actLine);
                    }
                    return;
                }
            }

            if (_isAct)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (_indexAct == 2)
                    {
                        _indexAct = 0;
                    }
                    else if (_indexAct == 3)
                    {
                        _indexAct = 1;
                    }
                }
                
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (_indexAct == 0)
                    {
                        _indexAct = 2;
                    }
                    else if (_indexAct == 1)
                    {
                        _indexAct = 3;
                    }
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (_indexAct == 0)
                    {
                        _indexAct = 1;
                    }
                    else if (_indexAct == 2)
                    {
                        _indexAct = 3;
                    }
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (_indexAct == 1)
                    {
                        _indexAct = 0;
                    }
                    else if (_indexAct == 3)
                    {
                        _indexAct = 2;
                    }
                }

                Soul.Instance.transform.position = transform.position + _indexAct switch
                {
                    0 => new Vector3(-6.179f, -1.177f),
                    1 => new Vector3(0.46f, -1.177f),
                    2 => new Vector3(-6.179f, -1.957f),
                    3 => new Vector3(0.46f, -1.957f),
                };
                
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    _sfxMenuSelect.Play();
                    _isActSelect = true;
                    _isAct = false;
                    _actSelect.SetActive(true);

                    foreach (var actLine in _actLines)
                    {
                        Destroy(actLine.gameObject);
                    }

                    _actLines = new List<SelectLine>();
                    
                    for (int i = 0; i < _enemies.Length; i++)
                    {
                        if (_enemies[i].IsSpare || _enemies[i].IsDead)
                            continue;
                        
                        var enemySelectLine = Instantiate(Resources.Load<EnemySelectLine>("Enemy Select Line"),
                            transform.position + new Vector3(-5.49f, i switch { 0 => -0.75f, 1 => -1.46f, 2 => -2.191f }, 0), Quaternion.identity, _actSelect.transform);
                        enemySelectLine.Init(_enemies[i].Name, _enemies[i].Health, _enemies[i].MaxHealth, _enemies[i].IsYellowName && !_enemies[i].IsSpare);
                        _actSelectLines.Add(enemySelectLine);
                    }
                    
                    return;
                }
                
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _isAct = false;
                    _act.SetActive(false);
                    _sfxMenuSelect.Play();

                    foreach (var line in _actLines)
                    {
                        Destroy(line.gameObject);
                    }

                    _actLines = new List<SelectLine>();

                    StartCoroutine(AwaitAct(_indexAct));
                    return;
                }
            }

            if (_isItemSelect)
            {
                var upgradeItems = false;
                
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (_indexItemSelect == 2)
                    {
                        _indexItemSelect = 0;
                    }
                    else if (_indexItemSelect == 3)
                    {
                        _indexItemSelect = 1;
                    }
                }
                
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (_indexItemSelect == 0)
                    {
                        _indexItemSelect = 2;
                    }
                    else if (_indexItemSelect == 1)
                    {
                        _indexItemSelect = 3;
                    }
                }
                
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (_pageNumber == 1)
                    {
                        if (_indexItemSelect == 0)
                        {
                            _indexItemSelect = 1;
                        }
                        else if (_indexItemSelect == 2)
                        {
                            _indexItemSelect = 3;
                        }
                        else if (_indexItemSelect == 1)
                        {
                            _indexItemSelect = 0;
                            _pageNumber = 2;
                            upgradeItems = true;
                            _sfxSelect.Play();
                        }
                        else if (_indexItemSelect == 3)
                        {
                            _indexItemSelect = 2;
                            _pageNumber = 2;
                            upgradeItems = true;
                            _sfxSelect.Play();
                        }
                    }
                    else
                    {
                        if (_indexItemSelect == 0)
                        {
                            _indexItemSelect = 1;
                        }
                        else if (_indexItemSelect == 2)
                        {
                            _indexItemSelect = 3;
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (_pageNumber == 1)
                    {
                        if (_indexItemSelect == 1)
                        {
                            _indexItemSelect = 0;
                        }
                        else if (_indexItemSelect == 3)
                        {
                            _indexItemSelect = 2;
                        }
                    }
                    else
                    {
                        if (_indexItemSelect == 1)
                        {
                            _indexItemSelect = 0;
                        }
                        else if (_indexItemSelect == 3)
                        {
                            _indexItemSelect = 2;
                        }
                        else if (_indexItemSelect == 0)
                        {
                            _indexItemSelect = 1;
                            _sfxSelect.Play();
                            _pageNumber = 1;
                            upgradeItems = true;
                        }
                        else if (_indexItemSelect == 2)
                        {
                            _indexItemSelect = 3;
                            _sfxSelect.Play();
                            _pageNumber = 1;
                            upgradeItems = true;
                        }
                    }
                }
                
                if (upgradeItems)
                {
                    _itemPageLabel.text = _pageNumber == 1 ? "PAGE 1" : "PAGE 2";

                    for (var index = 0; index < _itemSelectLines.Count; index++)
                    {
                        var item = _itemSelectLines[index];
                        item.Init(_pageNumber == 1
                            ? index switch
                            {
                                0 => SaveSystem.GetString("Item1"),
                                1 => SaveSystem.GetString("Item2"),
                                2 => SaveSystem.GetString("Item3"),
                                3 => SaveSystem.GetString("Item4"),
                            }
                            : index switch
                            {
                                0 => SaveSystem.GetString("Item5"),
                                1 => SaveSystem.GetString("Item6"),
                                2 => SaveSystem.GetString("Item7"),
                                3 => SaveSystem.GetString("Item8"),
                            });
                    }
                }

                Soul.Instance.transform.position = transform.position + _indexItemSelect switch
                {
                    0 => new Vector3(-6.179f, -1.177f),
                    1 => new Vector3(0.46f, -1.177f),
                    2 => new Vector3(-6.179f, -1.957f),
                    3 => new Vector3(0.46f, -1.957f),
                };
                
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    _sfxMenuSelect.Play();
                    _isMainMenu = true;
                    _isItemSelect = false;
                    _itemSelect.SetActive(false);
                    WriteStartMessage();

                    foreach (var itemLine in _itemSelectLines)
                    {
                        Destroy(itemLine.gameObject);
                    }

                    _itemSelectLines = new List<ItemLine>();
                    return;
                }
                
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    var itemCellID = _pageNumber == 1
                        ? _indexItemSelect switch
                        {
                            0 => "Item1",
                            1 => "Item2",
                            2 => "Item3",
                            3 => "Item4",
                        }
                        : _indexItemSelect switch
                        {
                            0 => "Item5",
                            1 => "Item6",
                            2 => "Item7",
                            3 => "Item8",
                        };
                    
                    if (SaveSystem.GetString(itemCellID) == string.Empty)
                        return;
                        
                    _isItemSelect = false;
                    _itemSelect.SetActive(false);
                    _sfxMenuSelect.Play();

                    foreach (var line in _itemSelectLines)
                    {
                        Destroy(line.gameObject);
                    }

                    _itemSelectLines = new List<ItemLine>();
                    
                    StartCoroutine(AwaitItem(itemCellID));
                    return;
                }
            }

            if (_isMercySelect)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    _indexMercySelect -= 1;

                    if (_indexMercySelect < 0)
                        _indexMercySelect = 0;
                }
                
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    _indexMercySelect += 1;
                    
                    if (_indexMercySelect > _mercySelectLines.Count - 1)
                        _indexMercySelect = _mercySelectLines.Count - 1;
                }
                
                Soul.Instance.transform.position = transform.position + _indexMercySelect switch
                {
                    0 => new Vector3(-6.178f, -1.177f),
                    1 => new Vector3(-6.178f, -1.908f),
                };
                
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    _sfxMenuSelect.Play();
                    _isMainMenu = true;
                    _isMercySelect = false;
                    WriteStartMessage();

                    foreach (var mercyLine in _mercySelectLines)
                    {
                        Destroy(mercyLine.gameObject);
                    }

                    _mercySelectLines = new List<SelectLine>();
                    return;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _isMercySelect = false;
                    _mercySelect.SetActive(false);
                    _sfxMenuSelect.Play();

                    foreach (var line in _mercySelectLines)
                    {
                        Destroy(line.gameObject);
                    }

                    _mercySelectLines = new List<SelectLine>();

                    if (_indexMercySelect == 0)
                    {
                        StartCoroutine(AwaitMercy());
                    }
                    else
                    {
                        StartCoroutine(AwaitRun());
                    }
                    
                    return;
                }
            }
        }
    }

    public void WriteStartMessage()
    {
        if (_writeCoroutine != null)
        {
            StopCoroutine(_writeCoroutine);
            _writeCoroutine = null;
        }

        _writeCoroutine = StartCoroutine(AwaitWrite());
    }

    public IEnumerator AwaitWrite()
    {
        _line.gameObject.SetActive(true);
        _line.text = string.Empty;
        
        for (int i = 0; i < BattleApproachMessage.Length; i++)
        {
            if (!_writeSFX.isPlaying)
                _writeSFX.Play();
            
            _line.text += BattleApproachMessage[i];
            yield return new WaitForSeconds(0.05f);
        }

        _writeCoroutine = null;
    }

    private IEnumerator AwaitFightIndicator()
    {
        _isTurnPlayer = false;
        _isFightIndicator = true;
        _fightIndicator.SetActive(true);
        Soul.Instance.gameObject.SetActive(false);
        _indicatorStick.localPosition = new Vector3(-6.72f, _indicatorStick.localPosition.y);
        _gunAnimator.transform.position = new Vector3(_enemies[_indexFightSelect].transform.position.x + 4, _gunAnimator.transform.position.y);
        yield return null;
        
        while (_indicatorStick.localPosition.x < 8.32f)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                break;
            }
            
            _indicatorStick.position += new Vector3(Time.deltaTime * 12, 0);
            yield return null;
        }

        _indicatorStick.GetComponent<Animator>().SetBool("IsActivate", true);
        _gunAnimator.SetBool("IsShot", true);
        _shotSFX.Play();
        yield return new WaitForSeconds(0.2f);
        _gunAnimator.SetBool("IsShot", false);
        yield return new WaitForSeconds(0.8f);
        _damageSFX.Play();
        var damage = _enemies[_indexFightSelect].IsYellowName ? _enemies[_indexFightSelect].Health : 3;
        var damageEffect = Instantiate(Resources.Load<DamageEffect>("Damage Effect"), 
            new Vector3(_enemies[_indexFightSelect].transform.position.x, transform.position.y + 5.01f), 
            Quaternion.identity, transform);
        damageEffect.Init((Mathf.Min(damage, _enemies[_indexFightSelect].Health)).ToString(), _enemies[_indexFightSelect].Health - damage, _enemies[_indexFightSelect].MaxHealth);
        StartCoroutine(_enemies[_indexFightSelect].AwaitDamage(damage));
        yield return new WaitForSeconds(1);
        Destroy(damageEffect.gameObject);
        _indicatorStick.GetComponent<Animator>().SetBool("IsActivate", false);
        _fightIndicator.SetActive(false);
        _isFightIndicator = false;

        // if (_enemies[_indexFightSelect].IsDead)
        //     yield return new WaitForSeconds(1);
        
        if (_enemies.Any(enemy => enemy.Health > 0))
            StartCoroutine(_enemies[_indexFightSelect].AwaitFight());
        else
            StartCoroutine(AwaitEndBattle());
    }

    public void PlayerTurn()
    {
        _isMainMenu = true;
        _indexAct = 0;
        Soul.Instance.enabled = false;
        StartCoroutine(AwaitPlayerTurn());
    }

    private IEnumerator AwaitPlayerTurn()
    {
        Soul.Instance.gameObject.SetActive(false);
        yield return _frame.AwaitUpgradeSize(4.8f, 1.15f);
        WriteStartMessage();
        _isTurnPlayer = true;
        yield return null;
        Soul.Instance.gameObject.SetActive(true);
    }

    private IEnumerator AwaitItem(string itemCellID)
    {
        _isTurnPlayer = false;
        var itemName = SaveSystem.GetString(itemCellID);

        if (itemCellID != string.Empty)
        {
            SaveSystem.SetString(itemCellID, string.Empty);

            BattleApproachMessage = $"* Вы использовали {itemName}\n* Вы восстановили 0 ОЗ";
            WriteStartMessage();

            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            if (IsGetWriteProcessing)
            {
                ShowWriteAllLine();
                yield return null;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }

            StopWrite();
        }
        
        yield return _enemies[_indexItemSelect].AwaitItem(itemName);
    }

    private IEnumerator AwaitAct(int act)
    {
        _isTurnPlayer = false;
        yield return _enemies[_indexActSelect].AwaitAct(act);
    }

    private IEnumerator AwaitMercy()
    {
        _isTurnPlayer = false;
        
        Soul.Instance.gameObject.SetActive(false);
        var counterWrapper = new[] { 0 };
        
        foreach (var enemy in _enemies)
        {
            if (enemy.IsYellowName && !enemy.IsSpare)
            {
                counterWrapper[0]++;
                enemy.StartCoroutine(enemy.AwaitSpare(() => counterWrapper[0]--));
            }
        }
        
        yield return new WaitUntil(() => counterWrapper[0] == 0);
        
        var isAllSpared = true;

        foreach (var enemy in _enemies)
        {
            if (!enemy.IsSpare)
                isAllSpared = false;
        }
        
        if (isAllSpared)
            StartCoroutine(AwaitEndBattle());
        else
            yield return _enemies[0].AwaitMercy();
    }

    private IEnumerator AwaitRun()
    {
        _isTurnPlayer = false;
        _isRun = true;
        yield return new WaitForSeconds(1);
        StartCoroutine(AwaitEndBattle());
    }

    public IEnumerator AwaitEndBattle()
    {
        Soul.Instance.gameObject.SetActive(false);
        
        _spareSFX.Play();
        var australium = 0;

        if (!_isRun)
        {
            foreach (var enemy in _enemies)
            {
                australium += enemy.Australium;
            }
        }

        BattleApproachMessage = $"YOU WON!\nYou earned {australium} Australium.";

        WriteStartMessage();
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (IsGetWriteProcessing)
        {
            ShowWriteAllLine();
            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }
        
        Instantiate(Resources.Load<EndBattleAnimation>("End Battle Animation"));
        MusicPlayer.Instance.Play(_previousMusic);

        foreach (var enemy in _enemies)
        {
            enemy.End();
        }
    }

    public void StopWrite()
    {
        if (_writeCoroutine != null)
        {
            StopCoroutine(_writeCoroutine);
        }
        
        _writeCoroutine = null;
        _line.gameObject.SetActive(false);
    }

    public void ShowWriteAllLine()
    {
        if (_writeCoroutine != null)
        {
            StopCoroutine(_writeCoroutine);
        }

        _line.text = BattleApproachMessage;
        _writeCoroutine = null;
    }
}