using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SketchFleets.LanguageSystem;

namespace SketchFleets
{
    /// <summary>
    /// Used to localize dropdown options
    /// </summary>
    public class LocalizableDropdown : MonoBehaviour 
    {
        #region Attribute
        [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
        [System.Serializable]
        public class OptionData : PropertyAttribute
        {
            public string value;
        }
        #endregion

        #region Public Fields
        public List<OptionData> options;
        #endregion

        #region Unity Callbacks
        private void OnEnable() 
        {
            LanguageManager.OnLanguageChanged += UpdateLocalization; 
            UpdateLocalization();   
        }

        private void OnDisable() 
        {
            LanguageManager.OnLanguageChanged -= UpdateLocalization; 
        }

        private void OnDestroy()
        {
            LanguageManager.OnLanguageChanged -= UpdateLocalization;
        }
        #endregion

        #region Localization
        public void UpdateLocalization()
        {
            int value = GetComponent<TMPro.TMP_Dropdown>().value;

            List<TMPro.TMP_Dropdown.OptionData> translated = new List<TMPro.TMP_Dropdown.OptionData>();

            foreach(var t in options)
                translated.Add(new TMPro.TMP_Dropdown.OptionData(LanguageManager.Localize(t.value)));

            GetComponent<TMPro.TMP_Dropdown>().ClearOptions();
            GetComponent<TMPro.TMP_Dropdown>().AddOptions(translated);

            GetComponent<TMPro.TMP_Dropdown>().value = value;
        }
        #endregion
    }
}
