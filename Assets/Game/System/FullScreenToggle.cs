using System.Collections.Generic;
using UnityEngine;

public sealed class FullScreenToggle : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            ToggleFullScreen();
        }
    }
    
    private void ToggleFullScreen()
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
            Screen.SetResolution(640, 480, FullScreenMode.Windowed);
        }
        else
        {
            Resolution[] resolutions = Screen.resolutions;
            Resolution maxResolution = resolutions[^1];
            int targetHeight = maxResolution.height;
            int targetWidth = targetHeight * 4 / 3;
            Resolution best4x3 = FindClosest4x3Resolution(targetWidth, targetHeight);
            Screen.SetResolution(best4x3.width, best4x3.height, FullScreenMode.FullScreenWindow);
        }
    }

    private Resolution FindClosest4x3Resolution(int targetWidth, int targetHeight)
    {
        Resolution[] allResolutions = Screen.resolutions;
        List<Resolution> fourByThreeResolutions = new List<Resolution>();

        // Фильтруем только разрешения с соотношением 4:3
        foreach (Resolution res in allResolutions)
        {
            float ratio = (float)res.width / res.height;

            // Допускаем небольшую погрешность для дробных соотношений
            if (Mathf.Abs(ratio - 1.333f) < 0.01f) // 4:3 = 1.333...
            {
                fourByThreeResolutions.Add(res);
            }
        }

        // Если нашли разрешения 4:3, ищем ближайшее к целевому
        if (fourByThreeResolutions.Count > 0)
        {
            Resolution closest = fourByThreeResolutions[0];
            float minDistance = float.MaxValue;

            foreach (Resolution res in fourByThreeResolutions)
            {
                // Вычисляем "расстояние" до целевого разрешения
                float distance = Mathf.Abs(res.width - targetWidth) +
                                 Mathf.Abs(res.height - targetHeight);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = res;
                }
            }

            Debug.Log($"Найдено ближайшее 4:3 разрешение: {closest.width}×{closest.height}");
            return closest;
        }

        // Если разрешения 4:3 не найдены, используем стандартные 4:3 разрешения
        Debug.LogWarning("Разрешения 4:3 не найдены, используется резервный вариант");
        return GetStandard4x3Resolution(targetHeight);
    }

    private static Resolution GetStandard4x3Resolution(int targetHeight)
    {
        Resolution resolution = new Resolution();

        // Стандартные разрешения 4:3
        switch (targetHeight)
        {
            case int h when h >= 1200:
                resolution.width = 1600;
                resolution.height = 1200;
                break;
            case int h when h >= 1024:
                resolution.width = 1366; // 1366×1024 ≈ 4:3
                resolution.height = 1024;
                break;
            case int h when h >= 768:
                resolution.width = 1024;
                resolution.height = 768;
                break;
            case int h when h >= 600:
                resolution.width = 800;
                resolution.height = 600;
                break;
            default:
                resolution.width = 640;
                resolution.height = 480;
                break;
        }

        return resolution;
    }
}
