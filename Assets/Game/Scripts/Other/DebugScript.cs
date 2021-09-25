using System;
using System.Collections;
using System.Collections.Generic;
using SketchFleets.SettingsSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace SketchFleets
{
    public class DebugScript : MonoBehaviour
    {
        private GUIStyle style = new GUIStyle();
        //[HideInInspector]
        public List<string> debugText = new List<string>();
        public PlayerInput pi;
        void Start()
        {
            style.normal.textColor = Color.black;
            style.fontSize = (int)(15*((float)Screen.currentResolution.width/1000));
        }

        public void UpdateDebug(string text, int n)
        {
            try
            {
                debugText[n] = text;
            }
            catch
            {
                DebugAdd(n);
                debugText[n-1] = text;
            }
        }

        private void DebugAdd(int n)
        {
            for (int i = debugText.Count; i <= n; i++)
            {
                debugText.Add("");
            }
        }

        void OnGUI()
        {
            if(PlayerPrefs.GetInt("debugMode") == 0) return;

            if (SceneManager.GetActiveScene().name == "Menu")
            {
                GUI.Label(new Rect(10, 15, 512, 50),
                    $"Version: {Application.version} - {Application.productName}/{Application.companyName}\n" +
                    $"Device: {SystemInfo.deviceModel} / {SystemInfo.operatingSystem}\n" +
                    $"Screen: {Screen.currentResolution}\n" +
                    $"CPU: {SystemInfo.processorFrequency}Mhz - {SystemInfo.processorType}\n" +
                    $"RAM: {SystemInfo.systemMemorySize/1024}Gb\n" +
                    $"GPU: {SystemInfo.graphicsDeviceName} / {SystemInfo.graphicsDeviceType} / {SystemInfo.graphicsMemorySize / 1024}Gb\n" +
                    $"Battery: {(int)(SystemInfo.batteryLevel*100)}% - {SystemInfo.batteryStatus}",style);
            }
            for (int i = 0; i < debugText.Count; i++)
            {
                GUI.Label(new Rect(10, 50+(i*style.fontSize)+10, 512, 50), debugText[i], style);
            }
        }
    }
}
