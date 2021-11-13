using System.Collections;
using UnityEngine;
using System.IO;

namespace SketchFleets.ProfileSystem
{
    /// <summary>
    /// Main profile system
    /// </summary>
    public class Profile
    {
        #region Private Fields
        private static readonly string FilePath = Application.persistentDataPath + "/data/" + "save.data";
        private static ProfileData data;
        private static bool runningThread = false;
        private static MonoBehaviour behaviour;
        #endregion

        #region Properties
        public static ProfileData Data {get => data; }
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
            Debug.Log(FilePath);
            return File.Exists(FilePath);
        }

        /// <summary>
        /// Get data
        /// </summary>
        /// <returns></returns>
        public static ProfileData GetData()
        {
            if(data == null)
                data = new ProfileData();

            return data;
        }

        /// <summary>
        /// Read profile from bytes
        /// </summary>
        /// <param name="callback"></param>
        public static void LoadProfile(System.Action<ProfileData> callback)
        {
            if (!Directory.Exists(Application.persistentDataPath + "/data"))
                Directory.CreateDirectory(Application.persistentDataPath + "/data");

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
        public static void SaveProfile(System.Action<ProfileData> callback)
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
        public static IEnumerator _Load(System.Action<ProfileData> callback)
        {
            while(runningThread)
                yield return new WaitForEndOfFrame();
            var thread = new System.Threading.Thread(() => {       
                GetData().saveObject = JsonUtility.FromJson<SaveObject>(File.ReadAllText(FilePath));
                runningThread = false;
            });

            runningThread = true;
            thread.Start();

            while(runningThread)
                yield return new WaitForSeconds(0.05f);

            GetData().ReloadInventories();

            Debug.Log("Saved Loaded:" + File.ReadAllText(FilePath) + " " + GetData().saveObject.mapData.seed);

            if(callback != null)
                callback(GetData());
        }

        /// <summary>
        /// File save coroutine (Save save bytes to file)
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static IEnumerator _Save(System.Action<ProfileData> callback)
        {
            while(runningThread)
                yield return new WaitForEndOfFrame();

            var thread = new System.Threading.Thread(() => {
                try
                {
                    //Save data
                    GetData().SaveInventories();
                
                    Debug.Log("Saving data to: " + FilePath);
                    File.WriteAllText(FilePath,JsonUtility.ToJson(GetData().saveObject));
                    runningThread = false;
                }
                catch(System.Exception e)
                {
                    Debug.Log(e);
                    //callback(GetData());
                }
            });
            
            runningThread = true;
            thread.Start();

            while(runningThread)
                yield return new WaitForSeconds(0.05f);

            if(callback != null)
                callback(GetData());
        }
        #endregion     
    }
}
