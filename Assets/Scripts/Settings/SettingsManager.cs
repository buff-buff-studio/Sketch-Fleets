using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SketchFleets.LanguageSystem;
using UnityEngine.Audio;

namespace SketchFleets.SettingsSystem
{   
    /// <summary>
    /// You can put this in a DontDestroyObjectOnLoad to auto load settings
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        #region Volume Controllers
        public AudioMixer volumeMasterMixer;
        public string volumeMasterParam;
        public AudioMixer volumeMusicMixer;
        public string volumeMusicParam;
        public AudioMixer volumeSfxMixer;
        public string volumeSfxParam;
        #endregion

        #region Static
        public static SettingsManager instance;
        #endregion

        private void Awake() 
        {
            ProfileSystem.Profile.Using(this);
        }

        void Start()
        {
            instance = this;
            //Default settings
            Settings.WithDefault<string>("language","enUS");
            Settings.WithDefault<int>("graphicsQuality",2);
            Settings.WithDefault<int>("resolution",1);
            Settings.WithDefault<float>("volume_master",0.5f);
            Settings.WithDefault<float>("volume_music",1f);
            Settings.WithDefault<float>("volume_sfx",1f);

            Settings.Load(this,() => {
                //Resolution
                SetResolution(Settings.Get<int>("resolution"));
                //Volumes
                SetVolume(volumeMasterParam,volumeMasterMixer,Settings.Get<float>("volume_master"));
                SetVolume(volumeMusicParam,volumeMusicMixer,Settings.Get<float>("volume_music"));
                SetVolume(volumeSfxParam,volumeSfxMixer,Settings.Get<float>("volume_sfx"));
                StartCoroutine(WaitToLoadLanguage());
            });
            DontDestroyOnLoad(gameObject);
        }

        IEnumerator WaitToLoadLanguage()
        {
            while(!LanguageManager.Loaded)
                yield return new WaitForEndOfFrame();

            //Change language
            LanguageManager.ChangeLanguage(Settings.Get<string>("language"));
        }

        public static void SetVolume(string param,AudioMixer mixer,float value)
        {
            mixer.SetFloat(param,Mathf.Log10(value) * 20);
        }

        public static void SetResolution(int resolution)
        {
            Settings.Set<int>("resolution",resolution);

            switch (resolution)
            {
                case 0:
                    Screen.SetResolution(1280,720,false);
                    break;   
                case 1:
                    Screen.SetResolution(1920,1080,false);
                    break;   
                case 2:
                    Screen.SetResolution(2560,1440,false);
                    break;                
            }
        }
    }
}
