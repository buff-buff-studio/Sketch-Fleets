using UnityEngine;
using UnityEngine.UI;
using SketchFleets.LanguageSystem;
using TMPro;

namespace SketchFleets
{
    /// <summary>
    /// Used to localize dropdown options
    /// </summary>
    public class LocalizableSelector : MonoBehaviour
    {
        #region Elements
        public Button buttonNext;
        public Button buttonPrev;
        private TMP_Text label;
        public string[] labels;
        public SettingsScreen settingsScreen;
        #endregion


        #region Public Fields
        public int value { get => _value; set { _value = Mod(value, labels.Length); UpdateLabel(); } }
        #endregion

        #region Internal Values
        private int _value;
        #endregion

        #region Core Methods
        private void UpdateLabel()
        {
            if(label != null)
                label.text = LanguageManager.Localize(labels[_value]);
        }
        #endregion

        #region Unity Callbacks
        private void OnEnable()
        {
            label = GetComponent<TMP_Text>();
            LanguageManager.OnLanguageChanged += UpdateLocalization;
            UpdateLocalization();
            UpdateLabel();

            if (buttonNext != null)
            {
                buttonNext.onClick.RemoveAllListeners();
                buttonNext.onClick.AddListener(() =>
                {
                    value++;
                    settingsScreen.OnChangeValue(1);
                });
            }

            if (buttonPrev != null)
            {
                buttonPrev.onClick.RemoveAllListeners();
                buttonPrev.onClick.AddListener(() =>
                {
                    value--;
                    settingsScreen.OnChangeValue(1);
                });
            }
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
        }
        #endregion


        #region Utils
        private int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }
        #endregion
    }
}