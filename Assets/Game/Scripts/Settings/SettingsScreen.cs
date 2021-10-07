using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SketchFleets.LanguageSystem;
using TMPro;
using SketchFleets.SettingsSystem;

namespace SketchFleets
{
    #region Utils
    public class OptionData : TMP_Dropdown.OptionData
    {
        #region Private Fields
        private string _value;
        #endregion

        #region Properties
        public string value { get => _value; }
        #endregion

        #region Constructors
        public OptionData(string name, string value) : base(name)
        {
            this._value = value;
        }
        #endregion
    }
    #endregion

    /// <summary>
    /// Settings back-end class
    /// </summary>
    public class SettingsScreen : MonoBehaviour
    {
        #region Dropdowns and Sliders IDS
        private const int LANGUAGE = 0;
        private const int GRAPHICS_QUALITY = 1;
        private const int RESOLUTION = 2;

        private const int VOLUME_MASTER = 3;
        private const int VOLUME_MUSIC = 4;
        private const int VOLUME_SFX = 5;

        private const int WINDOWS_MODE = 6;
        private const int VSYNC = 7;

        private const int TOUCH_RAY = 8;
        private const int DEBUG_MODE = 9;
        
        private const int CONTROLS_MODE = 10;
        private const int EVENTS_MODE = 11;
        #endregion

        #region Public Fields
        public TMP_Dropdown languageDropdown;
        public LocalizableSelector graphicsQuality;
        public TMP_Dropdown resolutionDropdown;
        public TMP_Dropdown windowsModeDropdown;
        public TMP_Dropdown controlsModeDropdown;
        public TMP_Dropdown eventsModeDropdown;
        public Toggle vsyncToggle;
        public Toggle touchRayToggle;
        public Toggle debugModeToggle;
        public Slider volumeMaster;
        public Slider volumeMusic;
        public Slider volumeSfx;
        #endregion

        #region Unity Callbacks
        private void OnEnable()
        {
            #region Language
            if (languageDropdown != null)
            {
                languageDropdown.ClearOptions();
                List<TMP_Dropdown.OptionData> data = new List<TMP_Dropdown.OptionData>();
                int value = 0;
                int i = 0;
                string lang = Settings.GetObject().language;
                foreach (Language s in LanguageManager.GetAllLanguages())
                {
                    data.Add(new OptionData(s.Name, s.Code));
                    if (s.Code == lang)
                        value = i;
                    i++;
                }
                languageDropdown.AddOptions(data);
                languageDropdown.value = value;
            }
            #endregion

            #region Graphics Quality
            if (graphicsQuality != null)
            {
                graphicsQuality.UpdateLocalization();
                graphicsQuality.value = Settings.GetObject().graphicsQuality;
            }
            #endregion

            #region Screen
            /*
            if(resolutionDropdown != null)
                resolutionDropdown.value = Settings.Get<int>("resolution");
            if(windowsModeDropdown != null)
            {
                windowsModeDropdown.GetComponent<LocalizableDropdown>().UpdateLocalization();
                windowsModeDropdown.value = Settings.Get<int>("winMode");
            }
            if(vsyncToggle != null)
                vsyncToggle.isOn = Settings.Get<bool>("vsync");
            SettingsManager.RefreshWindow();
            */
            #endregion

            #region Volume
            volumeMaster.value = Settings.GetObject().volumeMaster;
            volumeMusic.value = Settings.GetObject().volumeMusic;
            volumeSfx.value = Settings.GetObject().volumeSfx;
            #endregion

            #region Gameplay
            if (touchRayToggle != null)
                touchRayToggle.isOn = Settings.GetObject().touchRay;
            if (debugModeToggle != null)
                debugModeToggle.isOn = Settings.GetObject().debugMode;
            #endregion

            //FullScreenMode.Windowed
        }


        public void OnChangeValue(int dropdownId)
        {
            switch (dropdownId)
            {
                case LANGUAGE:
                    {
                        string language = ((OptionData)languageDropdown.options[languageDropdown.value]).value;
                        LanguageManager.ChangeLanguage(language);
                        Settings.GetObject().language = language;
                        Settings.Save();
                    }
                    break;

                case GRAPHICS_QUALITY:
                    {
                        QualitySettings.SetQualityLevel(graphicsQuality.value);
                        Settings.GetObject().graphicsQuality = graphicsQuality.value;
                        Settings.Save();
                    }
                    break;

                /*
                case RESOLUTION:
                {
                    SettingsManager.SetResolution(resolutionDropdown.value);
                }
                break;

                case WINDOWS_MODE:
                {
                    SettingsManager.SetWindowMode(windowsModeDropdown.value);
                }
                break;

                case VSYNC:
                {
                    Settings.Set<bool>("vsync",vsyncToggle.isOn);
                    SettingsManager.RefreshWindow();
                }
                break;
                */
                case VOLUME_MASTER:
                    {
                        Settings.GetObject().volumeMaster = volumeMaster.value;
                        SettingsManager.SetVolume(SettingsManager.instance.volumeMasterParam, SettingsManager.instance.volumeMasterMixer, volumeMaster.value);
                        Settings.Save();
                    }
                    break;

                case VOLUME_MUSIC:
                    {
                        Settings.GetObject().volumeMusic = volumeMusic.value;
                        SettingsManager.SetVolume(SettingsManager.instance.volumeMusicParam, SettingsManager.instance.volumeMusicMixer, volumeMusic.value);
                        Settings.Save();
                    }
                    break;

                case VOLUME_SFX:
                    {
                        Settings.GetObject().volumeSfx = volumeSfx.value;
                        SettingsManager.SetVolume(SettingsManager.instance.volumeSfxParam, SettingsManager.instance.volumeSfxMixer, volumeSfx.value);
                        Settings.Save();
                    }
                    break;

                case TOUCH_RAY:
                    {
                        Settings.GetObject().touchRay = touchRayToggle.isOn;
                        Settings.Save();
                    }
                    break;

                case DEBUG_MODE:
                    {
                        Settings.GetObject().debugMode = debugModeToggle.isOn;
                        Settings.Save();
                    }
                    break;
            }
        }
        #endregion
    }
}
