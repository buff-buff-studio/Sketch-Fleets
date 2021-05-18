using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    /// <summary>
    /// Used as a shortcut to create language buttons
    /// </summary>
    public class LanguageChanger : MonoBehaviour
    {
        /// <summary>
        /// Used to change current game language
        /// </summary>
        /// <param name="language"></param>
        public void ChangeLanguage(string language)
        {
            LanguageSystem.LanguageManager.ChangeLanguage(language);
        }
    }
}
