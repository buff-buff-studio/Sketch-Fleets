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
        public AudioSource menuMusicSource;
        public AudioSource storeMusicSource;
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
            /*
            Settings.WithDefault<string>("language","enUS");
            Settings.WithDefault<int>("graphicsQuality",2);
            Settings.WithDefault<int>("resolution",1);
            Settings.WithDefault<float>("volume_master",0.5f);
            Settings.WithDefault<float>("volume_music",1f);
            Settings.WithDefault<float>("volume_sfx",1f);
            Settings.WithDefault<bool>("vsync",true);
            */

            Settings.Load(this,() => {
                //Resolution
                SetResolution(Settings.GetObject().resolution);
                //Volumes
                SetVolume(volumeMasterParam,volumeMasterMixer,Settings.GetObject().volumeMaster);
                SetVolume(volumeMusicParam,volumeMusicMixer,Settings.GetObject().volumeMusic);
                SetVolume(volumeSfxParam,volumeSfxMixer,Settings.GetObject().volumeSfx);
                StartCoroutine(WaitToLoadLanguage());

                if(menuMusicSource != null)
                    if(menuMusicSource.isPlaying)
                    {
                        menuMusicSource.Stop();
                        menuMusicSource.Play();
                    }

                if(storeMusicSource != null)
                    if(storeMusicSource.isPlaying)
                    {
                        
                        storeMusicSource.Stop();
                        storeMusicSource.Play();
                    }

                menuMusicSource.volume = 1;
                storeMusicSource.volume = 1;

            });
            DontDestroyOnLoad(gameObject);
        }

        IEnumerator WaitToLoadLanguage()
        {
            while(!LanguageManager.Loaded)
                yield return new WaitForEndOfFrame();

            //Change language
            LanguageManager.ChangeLanguage(Settings.GetObject().language);
        }

        public static void SetVolume(string param,AudioMixer mixer,float value)
        {
            mixer.SetFloat(param,Mathf.Log10(value) * 20);
        }

        public static void SetResolution(int resolution)
        {
            Settings.GetObject().resolution = resolution;
            RefreshWindow();
        }

        public static void SetWindowMode(int mode)
        {
            Settings.GetObject().winMode = mode;
            RefreshWindow();
        }

        public static void RefreshWindow()
        {
            /*
            int resolution = Settings.Get<int>("resolution");
            FullScreenMode mode = (FullScreenMode) (Settings.Get<int>("winMode") + 1);
            int vsync = Settings.Get<bool>("vsync") ? 0 : 60;
            
            switch (resolution)
            {
                case 0:
                    Screen.SetResolution(1280,720,mode,vsync);
                    break;   
                case 1:
                    Screen.SetResolution(1920,1080,mode,vsync);
                    break;   
                case 2:
                    Screen.SetResolution(2560,1440,mode,vsync);
                    break;   
                case 3:
                    Screen.SetResolution(800,600,mode,vsync);
                    break;    
                case 4:
                    Screen.SetResolution(1024,768,mode,vsync);
                    break;    
                case 5:
                    Screen.SetResolution(2560,1080,mode,vsync);
                    break;                 
            }
            */
        }
    }
}
