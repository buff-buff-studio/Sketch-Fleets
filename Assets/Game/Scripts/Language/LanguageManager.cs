using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets.LanguageSystem
{
    /// <summary>
    ///     Main language system manager
    /// </summary>
    public class LanguageManager
    {
        #region Events

        public delegate void LanguageEvent();

        public static event LanguageEvent OnLanguageChanged;

        #endregion

        #region Private Fields

        private static readonly Dictionary<string, Language> languages = new Dictionary<string, Language>();
        private static Language currentLanguage;
        private static bool inited;
        public static bool Loaded { get; private set; }

        #endregion

        #region Main Methods

        /// <summary>
        ///     Try to change language and return a success bool indicator
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static bool ChangeLanguage(string language)
        {
            if (!ContainsLanguage(language))
            {
                return false;
            }

            if (currentLanguage != null)
            {
                if (currentLanguage == languages[language])
                {
                    return false;
                }
            }

            Language old = currentLanguage;

            //Changed language
            currentLanguage = languages[language];

            //Unbake language (Free memory)
            if (old != null && old != currentLanguage)
            {
                old.Unbake();
            }

            //Bake language
            currentLanguage.Bake();

            //Call handler
            if (OnLanguageChanged != null)
            {
                OnLanguageChanged();
            }

            return true;
        }

        /// <summary>
        ///     Check if language exists
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static bool ContainsLanguage(string language)
        {
            return languages.ContainsKey(language);
        }

        /// <summary>
        ///     Get current language
        /// </summary>
        /// <returns></returns>
        public static Language GetLanguage()
        {
            if (!inited)
            {
                Init();
            }

            return currentLanguage;
        }

        /// <summary>
        ///     Get all loaded languages
        /// </summary>
        /// <returns></returns>
        public static IEnumerable GetAllLanguages()
        {
            return languages.Values;
        }

        /// <summary>
        ///     Enumerate all language names
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> ListLanguages()
        {
            foreach (Language language in languages.Values)
            {
                yield return language.Localize("name");
            }
        }


        /// <summary>
        ///     Localize a string without arguments
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Localize(string key)
        {
            if (GetLanguage() == null)
            {
                return "...";
            }

            return GetLanguage().Localize(key);
        }

        /// <summary>
        ///     Localize a string with arguments
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Localize(string key, params string[] args)
        {
            if (GetLanguage() == null)
            {
                return Language.MissingEntry;
            }

            return GetLanguage().Localize(key, args);
        }

        #endregion

        #region Init

        /// <summary>
        ///     /// Load all languages
        /// </summary>
        public static void Init()
        {
            inited = true;
            var languages = Resources.LoadAll("Languages", typeof(TextAsset));

            //Iterate all languages
            foreach (Object t in languages)
            {
                //Variables
                string content = ((TextAsset)t).text;

                //Create language
                LanguageManager.languages[t.name] = new Language(t.name, content);

                if (content.StartsWith("name"))
                {
                    LanguageManager.languages[t.name].Name = content.Split('\n')[0].Split('=')[1];
                }
            }

            Loaded = true;
        }

        #endregion
    }
}