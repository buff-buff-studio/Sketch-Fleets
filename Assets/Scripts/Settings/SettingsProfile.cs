using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SketchFleets.SaveSystem;

namespace SketchFleets.SettingsSystem
{
    #region Delegates
    public delegate void OnParameterChange();
    #endregion

    /// <summary>
    /// Core settings class
    /// </summary>
    public class Settings
    {
        #region Static Fields
        private static Dictionary<string,object> defaultFields = new Dictionary<string, object>();
        private static readonly string FilePath = Application.persistentDataPath + "/" + "settings.data"; 
        private static bool loaded = false;
        private static Dictionary<string,OnParameterChange> eventHandlers = new Dictionary<string, OnParameterChange>();
        private static bool runningThread = false;
        private static MonoBehaviour behaviour;
        #endregion

        #region Private Fields
        private static Save save;
        #endregion

        #region Saving/Loading Handler
        private static Action SavingOrLoadingHandler;
        private static Action SavedOrLoadedHandler;
        #endregion

        #region Default
        /// <summary>
        /// Set a default value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        public static void WithDefault<T>(string key,T value)
        {
            defaultFields[key] = value;

            if(eventHandlers.ContainsKey(key))
                eventHandlers[key]();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Load all settings value
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="callback"></param>
        public static void Load(MonoBehaviour behaviour,Action callback)
        {
            //Behaviour
            Settings.behaviour = behaviour;

            if(loaded)
            {
                callback();
                return;
            }
            else
            {
                //Load and callback
                if(System.IO.File.Exists(FilePath))
                {
                    behaviour.StartCoroutine(_Load(callback));
                }
                else
                {
                    save = new Save();
                }
            }
        }

        /// <summary>
        /// Get parameter value
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>(string key)
        {     
            if(loaded)
            {
                if(save.HasKey(key))
                    return (T) save[key];
            }
                
            if(defaultFields.ContainsKey(key))
                return (T) defaultFields[key];

            return default(T);
        }

        /// <summary>
        /// Set a parameter value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        public static void Set<T>(string key,object value)
        {
            save[key] = value;

            if(eventHandlers.ContainsKey(key))
                eventHandlers[key]();

            behaviour.StartCoroutine(_Save());
        }

        /// <summary>
        /// Check if save contains a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Contains(string key)
        {
            return save.HasKey(key);
        }

        /// <summary>
        /// Reset all settings to default
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        public static void ResetToDefault<T>(string key)
        {
            List<PairPointer> pointers = new List<PairPointer>(save);
        
            save.Clear();
            behaviour.StartCoroutine(_Save());

            foreach(PairPointer pair in pointers)
            {
                if(eventHandlers.ContainsKey(pair.Key))
                    eventHandlers[pair.Key]();
            }
        }
        #endregion

        #region Handlers
        /// <summary>
        /// Add parameter change value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="handler"></param>
        public static void AddHandler(string key,OnParameterChange handler)
        {
            if(eventHandlers.ContainsKey(key))
                eventHandlers[key] = eventHandlers[key] + handler;
            else
                eventHandlers[key] = handler;
        }

        /// <summary>
        /// Clear all handlers for a parameter
        /// </summary>
        /// <param name="key"></param>
        public static void ClearHandlers(string key)
        {
            eventHandlers.Remove(key);
        }

        /// <summary>
        /// Clear all handler
        /// </summary>
        public static void ClearAllHandlers()
        {
            eventHandlers.Clear();
        }

        /// <summary>
        /// Set save and load handlers
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void SetSaveLoadHandlers(Action start,Action end)
        {
            SavingOrLoadingHandler = start;
            SavedOrLoadedHandler = end;
        }
        #endregion

        #region Threads
        public static IEnumerator _Load(Action callback)
        {
            if(SavingOrLoadingHandler != null)
                SavingOrLoadingHandler();

            while(runningThread)
                yield return new WaitForEndOfFrame();

            var thread = new System.Threading.Thread(() => {
                save = Save.FromBytes(File.ReadAllBytes(FilePath),EditMode.Fixed);

                runningThread = false;              
            });
            
            runningThread = true;
            thread.Start();

            while(runningThread)
                yield return new WaitForEndOfFrame();

            loaded = true;

            if(callback != null)
                callback();

            //Update save
            foreach(PairPointer pair in save)
            {
                if(eventHandlers.ContainsKey(pair.Key))
                    eventHandlers[pair.Key]();
            }

            if(SavedOrLoadedHandler != null)
                SavedOrLoadedHandler();
        }

        public static IEnumerator _Save()
        {
            if(SavingOrLoadingHandler != null)
                SavingOrLoadingHandler();

            while(runningThread)
                yield return new WaitForEndOfFrame();

            var thread = new System.Threading.Thread(() => {
                File.WriteAllBytes(FilePath,save.ToBytes());
                runningThread = false;
            });
            
            runningThread = true;
            thread.Start();

            while(runningThread)
                yield return new WaitForEndOfFrame();

            if(SavedOrLoadedHandler != null)
                SavedOrLoadedHandler();
        }
        #endregion
    }
}