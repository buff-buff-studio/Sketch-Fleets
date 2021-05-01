using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SketchFleets.SaveSystem;
using System.IO;

namespace SketchFleets.ProfileSystem
{
    /// <summary>
    /// Main profile system
    /// </summary>
    public class Profile
    {
        #region Private Fields
        private static readonly string FilePath = Application.persistentDataPath + "/" + "save.data";
        private static Save save;
        private static bool runningThread = false;
        private static MonoBehaviour behaviour;
        #endregion

        #region Init
        /// <summary>
        /// Init profile using a mono behaviour to start coroutines
        /// </summary>
        /// <param name="behaviour"></param>
        public static void Using(MonoBehaviour behaviour)
        {
            Profile.behaviour = behaviour;
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// Check if save profile exists
        /// </summary>
        /// <returns></returns>
        public static bool Exists()
        {
            return File.Exists(FilePath);
        }

        /// <summary>
        /// Get save
        /// </summary>
        /// <returns></returns>
        public static Save GetSave()
        {
            if(save == null)
                save = SketchFleets.SaveSystem.Save.NewDynamic();

            return save;
        }

        /// <summary>
        /// Read profile from bytes
        /// </summary>
        /// <param name="callback"></param>
        public static void LoadProfile(System.Action<Save> callback)
        {
            if(System.IO.File.Exists(FilePath))
            {
                behaviour.StartCoroutine(_Load(callback));
            }
            else
            {
                SaveProfile(callback);
            }
        }   

        /// <summary>
        /// Save profile to file
        /// </summary>
        /// <param name="callback"></param>
        public static void SaveProfile(System.Action<Save> callback)
        {
            behaviour.StartCoroutine(_Save(callback));
        }
        #endregion

        #region Coroutines
        /// <summary>
        /// File load coroutine (Parse file bytes to save)
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static IEnumerator _Load(System.Action<Save> callback)
        {
            while(runningThread)
                yield return new WaitForEndOfFrame();

            var thread = new System.Threading.Thread(() => {
                save = Save.FromBytes(File.ReadAllBytes(FilePath),EditMode.Dynamic);
                runningThread = false;
                if(callback != null)
                    callback(GetSave());
            });
            
            runningThread = true;
            thread.Start();
        }

        /// <summary>
        /// File save coroutine (Save save bytes to file)
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static IEnumerator _Save(System.Action<Save> callback)
        {
            while(runningThread)
                yield return new WaitForEndOfFrame();

            var thread = new System.Threading.Thread(() => {
                File.WriteAllBytes(FilePath,GetSave().ToBytes());
                runningThread = false;
                if(callback != null)
                    callback(GetSave());
            });
            
            runningThread = true;
            thread.Start();
        }
        #endregion     
    }
}
