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
    public sealed class SettingsManager : MonoBehaviour
    {
        #region Volume Controllers

        [Header("Audio Mixers")]
        public AudioMixer volumeMasterMixer;

        public string volumeMasterParam;
        public AudioMixer volumeMusicMixer;
        public string volumeMusicParam;
        public AudioMixer volumeSfxMixer;
        public string volumeSfxParam;

        [Header("Audio Sources")]
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

        private void Start()
        {
            instance = this;


            Settings.Load(this, () =>
            {
                //Resolution
                SetResolution(Settings.GetObject().resolution);
                //Volumes
                SetVolume(volumeMasterParam, volumeMasterMixer, Settings.GetObject().volumeMaster);
                SetVolume(volumeMusicParam, volumeMusicMixer, Settings.GetObject().volumeMusic);
                SetVolume(volumeSfxParam, volumeSfxMixer, Settings.GetObject().volumeSfx);
                StartCoroutine(WaitToLoadLanguage());

                if (menuMusicSource != null)
                {
                    if (menuMusicSource.isPlaying)
                    {
                        menuMusicSource.Stop();
                        menuMusicSource.Play();
                    }
                }

                if (storeMusicSource != null)
                {
                    if (storeMusicSource.isPlaying)
                    {
                        storeMusicSource.Stop();
                        storeMusicSource.Play();
                    }
                }

                menuMusicSource.volume = 1;
                storeMusicSource.volume = 1;
            });
            DontDestroyOnLoad(gameObject);
        }

        IEnumerator WaitToLoadLanguage()
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();

            while (!LanguageManager.Loaded)
            {
                yield return wait;
            }

            //Change language
            LanguageManager.ChangeLanguage(Settings.GetObject().language);
        }

        public static void SetVolume(string param, AudioMixer mixer, float value)
        {
            mixer.SetFloat(param, Mathf.Log10(value) * 20);
        }

        public static void SetResolution(int resolution)
        {
            Settings.GetObject().resolution = resolution;
        }

        public static void SetWindowMode(int mode)
        {
            Settings.GetObject().winMode = mode;
        }
    }
}