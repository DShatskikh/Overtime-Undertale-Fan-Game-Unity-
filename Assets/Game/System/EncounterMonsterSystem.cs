using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[Serializable]
public enum ZoneEnum
{
    None = -1,
    Hospital = 0
}

[Serializable]
public class EncounterZone
{
    public string zoneName;
    [Tooltip("Минимальные шаги до проверки")]
    public int minSteps = 80;
    [Tooltip("Максимальные шаги до проверки")]
    public int maxSteps = 300;
    [Tooltip("Базовый шанс встречи (0-100)")]
    [Range(0, 100)]
    public float baseEncounterChance = 30f;
    [Tooltip("Противники")]
    public BattleDataConfig[] Enemies;
}

public sealed class EncounterMonsterSystem : MonoBehaviour
{
    public static EncounterMonsterSystem Instance { get; private set; }
    
    [Header("Настройки зон")]
    public List<EncounterZone> encounterZones = new List<EncounterZone>();
    
    [Header("Настройки системы")]
    [Tooltip("Снижение шанса при долгом отсутствии встреч (0-50)")]
    [Range(0, 50)]
    public float mercyReduction = 10f;

    // Текущее состояние
    private int currentSteps;
    private int stepsThreshold;
    private ZoneEnum currentZoneEnum;
    private int failedAttempts = 0;
    private Vector3 lastPosition;

    private void Awake()
    {
        if (Instance != null)
            return;
            
        ChangeZone(ZoneEnum.None);
        Instance = this;
    }

    private void Start()
    {
        if (Constants.EncounterHospitalScenes.Contains(SceneManager.GetActiveScene().buildIndex))
        {
            ChangeZone(ZoneEnum.Hospital);
        }
    }

    private void Update()
    {
        CheckPlayerMovement();
        
        // Для тестирования
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log($"Шагов до встречи: {currentSteps}/{stepsThreshold}");
        }
    }

    // === Публичные методы для управления ===
    public void ChangeZone(ZoneEnum newZoneIndex)
    {
        Debug.Log("Change Zone " + newZoneIndex);
        
        if (currentZoneEnum != newZoneIndex)
        {
            InitializeZone(newZoneIndex);
        }
    }

    public void ForceEncounter(float bonusChance = 0f)
    {
        var zone = encounterZones[(int)currentZoneEnum];
        var chance = zone.baseEncounterChance + bonusChance;
        
        if (Random.Range(0f, 100f) < chance)
        {
            TriggerNormalEncounter();
        }
    }

    private void CheckPlayerMovement()
    {
        if (!Player.Instance)
            return;
        
        var currentPosition = Player.Instance.transform.position;
        
        // Проверяем, сдвинулся ли игрок
        if (!(Vector3.Distance(currentPosition, lastPosition) > 0.1f))
            return;
        
        // Учитываем бег (удержание Shift)
        var stepValue = Player.Instance.IsRunning ? 1.5f : 1;
        currentSteps -= Mathf.RoundToInt(stepValue);
        lastPosition = currentPosition;
            
        // Проверяем, не пора ли запустить проверку встречи
        if (currentSteps <= 0)
        {
            AttemptEncounter();
            ResetStepCounter();
        }
    }

    private void AttemptEncounter()
    {
        if (currentZoneEnum == ZoneEnum.None)
            return;
        
        var zone = encounterZones[(int)currentZoneEnum];
        
        // Вычисляем текущий шанс с учетом "правила милосердия"
        var currentChance = zone.baseEncounterChance;
        
        if (failedAttempts > 2)
        {
            currentChance = Mathf.Max(5f, currentChance - (mercyReduction * failedAttempts));
            Debug.Log($"Правило милосердия! Шанс снижен до {currentChance}%");
        }
        
        // Проверка на обычную встречу
        if (Random.Range(0f, 100f) < currentChance)
        {
            // Обычная встреча
            TriggerNormalEncounter();
            
            failedAttempts = 0; // Сброс после успешной встречи
        }
        else
        {
            // Встречи не произошло
            failedAttempts++;
            Debug.Log($"Проверка пройдена, встречи нет. Неудач подряд: {failedAttempts}");
        }
    }

    private void InitializeZone(ZoneEnum zoneIndex)
    {
        currentZoneEnum = zoneIndex;
        ResetStepCounter();
        failedAttempts = 0;
        
        if (currentZoneEnum != ZoneEnum.None)
            Debug.Log($"Вход в зону: {encounterZones[(int)zoneIndex].zoneName}");
    }

    private void ResetStepCounter()
    {
        if (currentZoneEnum == ZoneEnum.None)
            return;
        
        var zone = encounterZones[(int)currentZoneEnum];
        stepsThreshold = Random.Range(zone.minSteps, zone.maxSteps + 1);
        currentSteps = stepsThreshold;
    }

    private void TriggerNormalEncounter()
    {
        if (currentZoneEnum == ZoneEnum.None)
            return;

        Debug.Log("Начали битву");

        Player.Instance.enabled = false;
        StartCoroutine(AwaitStartBattle());
    }

    private IEnumerator AwaitStartBattle()
    {
        Player.Instance.ToggleWarning(true);
        yield return new WaitForSeconds(1f);
        Player.Instance.ToggleWarning(false);

        var enemies = encounterZones[(int)currentZoneEnum].Enemies;
        var startBattleAnimation = Instantiate(Resources.Load<StartBattleAnimation>("StartBattleAnimation"));
        startBattleAnimation.Init(11, new Vector2(-6.76f, -5.300001f), 
            () => FindAnyObjectByType<BattleController>().Init(enemies[Random.Range(0, enemies.Length)].Data));
    }
}