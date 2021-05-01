using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets.LanguageSystem
{
    /// <summary>
    /// Main language system manager
    /// </summary>
    public class LanguageManager
    {
        #region Private Fields
        private static Dictionary<string,Language> languages = new Dictionary<string, Language>();
        private static Language currentLanguage;
        #endregion

        #region Main Methods
        /// <summary>
        /// Try to change language and return a success bool indicator
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static bool ChangeLanguage(string language)
        {
            if(!ContainsLanguage(language))
                return false;

            Language old = currentLanguage;

            //Changed language
            currentLanguage = languages[language];

            //Unbake language (Free memory)
            if(old != null && old != currentLanguage)
                old.Unbake();

            //Bake language
            currentLanguage.Bake();

            return true;
        }

        /// <summary>
        /// Check if language exists
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static bool ContainsLanguage(string language)
        {
            return languages.ContainsKey(language);
        }

        /// <summary>
        /// Get current language
        /// </summary>
        /// <returns></returns>
        public static Language GetLanguage()
        {
            return currentLanguage;
        }

        /// <summary>
        /// Get all loaded languages
        /// </summary>
        /// <returns></returns>
        public static IEnumerable GetAllLanguages()
        {
            return languages.Values;
        }

        /// <summary>
        /// Localize a string without arguments
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Localize(string key)
        {
            if(currentLanguage == null)
                return Language.MissingEntry;
                
            return GetLanguage().Localize(key);
        }

        /// <summary>
        /// Localize a string with arguments
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Localize(string key,params string[] args)
        {
            if(currentLanguage == null)
                return Language.MissingEntry;

            return GetLanguage().Localize(key,args);
        }

        /// <summary>
        /// Localize entire UI
        /// </summary>
        /// <param name="canvas"></param>
        public static void LocalizeUI(Transform canvas)
        {
            
        }
        #endregion

        #region Init
        /// <summary>
        /// Load all languages
        /// </summary>
        public static void Init()
        {
            Debug.Log("Loading languages:");
            Object[] languages =  Resources.LoadAll("Languages", typeof(TextAsset));

            //Iterate all languages
            foreach (var t in languages)
            {
                //Variables
                string content = ((TextAsset) t).text;
                
                //Create language
                LanguageManager.languages[t.name] = new Language(t.name,content);
            }

            //Set current language
            ChangeLanguage("enUS");
        }
        #endregion
    }
}