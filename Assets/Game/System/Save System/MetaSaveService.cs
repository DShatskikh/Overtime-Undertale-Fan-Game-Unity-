using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class MetaSaveService
{
    private static string saveFolderPath;
    private static string currentSaveFileName = "meta save.txt";
    private static bool isInitialized = false;
    
    private static void Initialize()
    {
        if (isInitialized)
            return;
        
        // Создаем папку для сохранений
        saveFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyGame", "Save");
        
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
        
        isInitialized = true;
        Console.WriteLine($"Save system initialized. Save folder: {saveFolderPath}");
    }
    
    public static void SetInt(string key, int value) => Set(key, value.ToString());
    public static void SetFloat(string key, float value) => Set(key, value.ToString("F6"));
    public static void SetBool(string key, bool value) => Set(key, value.ToString());
    
    private static void Set(string key, string value)
    {
        if (!isInitialized)
            Initialize();
        
        string filePath = Path.Combine(saveFolderPath, currentSaveFileName);
        
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.WriteLine($"{key}-{value}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    
    private static string Get(string key, string defaultValue = "")
    {
        if (!isInitialized)
            Initialize();
        
        string filePath = Path.Combine(saveFolderPath, currentSaveFileName);
        
        if (!File.Exists(filePath))
        {
            return defaultValue;
        }
        
        try
        {
            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Разделяем строку по первому дефису
                    int separatorIndex = line.IndexOf('-');
                    if (separatorIndex > 0)
                    {
                        string inFileKey = line.Substring(0, separatorIndex);

                        if (inFileKey == key)
                        {
                            string value = line.Substring(separatorIndex + 1);
                            return value;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading game: {e.Message}");
        }

        return defaultValue;
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
}