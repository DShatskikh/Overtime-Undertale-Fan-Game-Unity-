using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Bootstrap : MonoBehaviour
{
    [DllImport("user32.dll", EntryPoint = "SetWindowText")]
    public static extern bool SetWindowText(IntPtr hwnd, string lpString);
    
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    
    private void Awake()
    {
        SetWindowText(GetActiveWindow(), "Happy birthday, Vlados! F4 - Fullscreen");
        SceneManager.LoadScene(1);
    }
}