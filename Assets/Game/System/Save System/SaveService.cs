using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class SaveSystem
{
    private static string saveFolderPath;
    private static string currentSaveFileName = "save.txt";
    private static Dictionary<string, string> saveData = new Dictionary<string, string>();
    private static bool isInitialized = false;

    // Инициализация системы (вызывается автоматически при первом использовании)
    private static void Initialize()
    {
        if (isInitialized)
            return;
        
        // Создаем папку для сохранений
        saveFolderPath = Path.Combine(Application.dataPath, "Save");
        
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
        
        isInitialized = true;
        Load();
        Debug.Log($"Save system initialized. Save folder: {saveFolderPath}");
    }

    // Установка пользовательского пути для сохранений (опционально)
    public static void SetSavePath(string customPath)
    {
        saveFolderPath = customPath;
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
        
        isInitialized = true;
    }

    // Сохранение значения в памяти
    private static void Set(string key, string value)
    {
        if (!isInitialized) Initialize();
        saveData[key] = value;
    }

    // Сохранение различных типов данных в памяти
    public static void SetInt(string key, int value) => Set(key, value.ToString());
    public static void SetFloat(string key, float value) => Set(key, value.ToString("F6"));
    public static void SetBool(string key, bool value) => Set(key, value.ToString());
    public static void SetString(string key, string value) => Set(key, value.ToString());

    // Получение значения из памяти
    private static string Get(string key, string defaultValue = "")
    {
        if (!isInitialized) Initialize();
        return saveData.ContainsKey(key) ? saveData[key] : defaultValue;
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
        string value = Get(key);
        return int.TryParse(value, out int result) ? result : defaultValue;
    }

    public static float GetFloat(string key, float defaultValue = 0f)
    {
        string value = Get(key);
        return float.TryParse(value, out float result) ? result : defaultValue;
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        string value = Get(key);
        return bool.TryParse(value, out bool result) ? result : defaultValue;
    }
    
    public static string GetString(string key, string defaultValue = "")
    {
        string value = Get(key);
        return value == string.Empty ? defaultValue : value;
    }
    
    // Сохранение всех данных из памяти в файл
    public static void Save()
    {
        if (!isInitialized) Initialize();
        
        string filePath = Path.Combine(saveFolderPath, currentSaveFileName);
        
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                foreach (var pair in saveData)
                {
                    writer.WriteLine($"{pair.Key}-{pair.Value}");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    // Загрузка всех данных из файла в память
    public static void Load()
    {
        if (!isInitialized)
            Initialize();
        
        string filePath = Path.Combine(saveFolderPath, currentSaveFileName);
        
        if (!File.Exists(filePath))
        {
            return;
        }
        
        try
        {
            saveData.Clear();
            
            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Разделяем строку по первому дефису
                    int separatorIndex = line.IndexOf('-');
                    if (separatorIndex > 0)
                    {
                        string key = line.Substring(0, separatorIndex);
                        string value = line.Substring(separatorIndex + 1);
                        saveData[key] = value;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading game: {e.Message}");
        }
    }

    // Проверка существования ключа в памяти
    public static bool HasKey(string key)
    {
        if (!isInitialized) Initialize();
        return saveData.ContainsKey(key);
    }

    // Удаление ключа из памяти
    public static void DeleteKey(string key)
    {
        if (!isInitialized) Initialize();
        if (saveData.ContainsKey(key))
        {
            saveData.Remove(key);
        }
    }

    // Удаление файла сохранения
    public static void DeleteSave()
    {
        if (!isInitialized) Initialize();
        
        string filePath = Path.Combine(saveFolderPath, currentSaveFileName);
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            saveData.Clear();
        }
    }

    // Получение всех ключей из памяти
    public static string[] GetAllKeys()
    {
        if (!isInitialized) Initialize();
        string[] keys = new string[saveData.Count];
        saveData.Keys.CopyTo(keys, 0);
        return keys;
    }

    // Очистка всех данных из памяти (без удаления файла)
    public static void ClearMemory()
    {
        saveData.Clear();
    }
}