using System;
using UnityEngine;

public sealed class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public string PlayerName;
    public float HP;
    public float MaxHP;
    public int LV;
    public int XP;
    public int KILLED;
    public int HIRED;
    public int AUSTRALIUM;
    public string Weapon;
    public string Armor;

    public event Action<float, float> ChangeHealth;
        
    private void Awake()
    {
        Instance = this;
            
        PlayerName = SaveSystem.GetString("PlayerName", "Miss");
        HP = SaveSystem.GetFloat("HP", 20);
        MaxHP = SaveSystem.GetFloat("MaxHP", 20);
        LV = SaveSystem.GetInt("LV", 1);
        XP = SaveSystem.GetInt("XP", 0);
        KILLED = SaveSystem.GetInt("KILLED", 0);
        HIRED = SaveSystem.GetInt("HIRED", 0);
        AUSTRALIUM = SaveSystem.GetInt("AUSTRALIUM", 0);
    }

    public void UpdateHP()
    {
        ChangeHealth?.Invoke(HP, MaxHP);
    }

    public void Save()
    {
         SaveSystem.SetString("PlayerName", PlayerName);
         SaveSystem.SetFloat("HP", HP);
         SaveSystem.SetFloat("MaxHP", MaxHP);
         SaveSystem.SetInt("LV", LV);
         SaveSystem.SetInt("XP", XP);
         SaveSystem.SetInt("KILLED", KILLED);
         SaveSystem.SetInt("HIRED", HIRED);
         SaveSystem.SetInt("AUSTRALIUM", AUSTRALIUM);
    }
}
