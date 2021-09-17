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
        public string value { get => _value;}
        #endregion

        #region Constructors
        public OptionData(string name,string value) : base(name)
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
        #endregion

        #region Public Fields
        public TMP_Dropdown languageDropdown;
        public TMP_Dropdown graphicsQuality;
        public TMP_Dropdown resolutionDropdown;
        public TMP_Dropdown windowsModeDropdown;
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
            if(languageDropdown != null)
            {
                languageDropdown.ClearOptions();
                List<TMP_Dropdown.OptionData> data = new List<TMP_Dropdown.OptionData>();
                int value = 0;
                int i = 0;
                string lang = Settings.Get<string>("language");
                foreach(Language s in LanguageManager.GetAllLanguages())
                {
                    data.Add(new OptionData(s.Name,s.Code));
                    if(s.Code == lang) 
                        value = i;
                    i ++;
                }
                languageDropdown.AddOptions(data);
                languageDropdown.value = value;
            }
            #endregion

            #region Graphics Quality
            if(graphicsQuality != null)
            {
                graphicsQuality.GetComponent<LocalizableDropdown>().UpdateLocalization();
                graphicsQuality.value = Settings.Get<int>("graphicsQuality");
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
            volumeMaster.value = Settings.Get<float>("volume_master");
            volumeMusic.value = Settings.Get<float>("volume_music");
            volumeSfx.value = Settings.Get<float>("volume_sfx");
            #endregion
            
            #region Gameplay
            if(touchRayToggle != null)
                touchRayToggle.isOn = Settings.Get<bool>("touchRay");
            if(debugModeToggle != null)
                debugModeToggle.isOn = Settings.Get<bool>("debugMode");
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
                    Settings.Set<string>("language",language);
                }
                break;

                case GRAPHICS_QUALITY:
                {
                    QualitySettings.SetQualityLevel(graphicsQuality.value);
                    Settings.Set<string>("graphicsQuality",graphicsQuality.value);
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
                    Settings.Set<string>("volume_master",volumeMaster.value);
                    SettingsManager.SetVolume(SettingsManager.instance.volumeMasterParam,SettingsManager.instance.volumeMasterMixer,volumeMaster.value);
                }
                break;

                case VOLUME_MUSIC:
                {
                    Settings.Set<string>("volume_music",volumeMusic.value);
                    SettingsManager.SetVolume(SettingsManager.instance.volumeMusicParam,SettingsManager.instance.volumeMusicMixer,volumeMusic.value);
                }
                break;

                case VOLUME_SFX:
                {
                    Settings.Set<string>("volume_sfx",volumeSfx.value);
                    SettingsManager.SetVolume(SettingsManager.instance.volumeSfxParam,SettingsManager.instance.volumeSfxMixer,volumeSfx.value);
                }
                break;
                
                case TOUCH_RAY:
                {
                    Settings.Set<bool>("touchRay",touchRayToggle.isOn);
                } 
                break;
                
                case DEBUG_MODE:
                {
                    Settings.Set<bool>("debugMode",debugModeToggle.isOn);
                }
                break;
            }
        }
        #endregion
    }
}
