using UnityEngine;
using SketchFleets.LanguageSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SketchFleets
{  
    /// <summary>
    /// Localizable text component
    /// </summary>
    [DisallowMultipleComponent]
    public class LocalizableText : MonoBehaviour
    {
        #region Public Fields
        /// <summary>
        /// Holds original string to keep translating
        /// </summary>
        public string originalText = "";
        #endregion

        #region Private Fields
        /// <summary>
        /// Used to access Text/TMP_Text text
        /// </summary>
        /// <value></value>
        private string Text {
            get{
                UnityEngine.UI.Text text;
                if((text = GetComponent<UnityEngine.UI.Text>()) != null)
                    return text.text;

                TMPro.TMP_Text textb;
                if((textb = GetComponent<TMPro.TMP_Text>()) != null)
                    return textb.text;

                return "";
            }
            set{
                UnityEngine.UI.Text text;
                if((text = GetComponent<UnityEngine.UI.Text>()) != null)
                    text.text = value;

                TMPro.TMP_Text textb;
                if((textb = GetComponent<TMPro.TMP_Text>()) != null)
                    textb.text = value;
            }
        }
        #endregion

        #region UnityCallbacks
        /// <summary>
        /// Save text original text
        /// </summary>
        void OnEnable() 
        {
            this.originalText = Text;

            //Update
            UpdateLocalization();

            //Add event handler
            LanguageManager.OnLanguageChanged += UpdateLocalization;
        }

        /// <summary>
        /// Clear all data and remove event handlers
        /// </summary>
        void OnDisable() 
        {
            Text = this.originalText;
            this.originalText = null;
             
            //Remove event handler
            LanguageManager.OnLanguageChanged -= UpdateLocalization;
        }

        private void OnDestroy() 
        {
            //Remove event handler
            LanguageManager.OnLanguageChanged -= UpdateLocalization;
        }
        
        #endregion

        #region EventHandler
        /// <summary>
        /// On language change event handler
        /// </summary>
        public void UpdateLocalization()
        {
            if(GetComponent<UnityEngine.UI.Text>() == null && GetComponent<TMPro.TMP_Text>() == null)
            {
                Destroy(this);
                return;
            }

            

            Text = System.Text.RegularExpressions.Regex.Replace(originalText, @"(?<!\\){([^}]+)\}", m => LanguageManager.Localize(m.Groups[1].Value));
        }
        #endregion
    }

    #if UNITY_EDITOR
    /// <summary>
    /// Custom editor for localizable text
    /// </summary>
    [CustomEditor(typeof(LocalizableText))]
    public class LocalizableTextEditor : Editor
    {
        /// <summary>
        /// Used to render custom inspector for LocalizableText 
        /// </summary>
        public override void OnInspectorGUI()
        {
            LocalizableText text = (LocalizableText)target;
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField("Unlocalized Text");
            EditorGUILayout.TextArea(text.originalText, GUILayout.Height(40));
            EditorGUI.EndDisabledGroup();
        }
    }
    
    /// <summary>
    /// Add localizable text option 
    /// </summary>
    public static class LocalizableTextAdder 
    {
        [MenuItem("GameObject/UI/Text - Localizable", false, 100)]
        /// <summary>
        /// Add prefab creation option
        /// </summary>
        static void Init() {
            GameObject go = new GameObject("Text (Localizable)");
            LocalizableText loc = go.AddComponent<LocalizableText>();
            UnityEngine.UI.Text txt = go.AddComponent<UnityEngine.UI.Text>();
            txt.text = "{ui_string}";
            if(Selection.transforms.Length > 0)
                go.transform.SetParent(Selection.transforms[0]);

            RectTransform rt;
            if((rt = go.GetComponent<RectTransform>()) != null)
            {
                rt.anchoredPosition = Vector3.zero;
            }
            
            UnityEditor.Selection.objects = new Object[] { go };
        }

        [MenuItem("GameObject/UI/Text - (TMP) Localizable", false, 100)]
        /// <summary>
        /// Add prefab creation option
        /// </summary>
        static void InitB() {
            GameObject go = new GameObject("Text (Localizable)");
            LocalizableText loc = go.AddComponent<LocalizableText>();
            TMPro.TextMeshProUGUI txt = go.AddComponent<TMPro.TextMeshProUGUI>();
            txt.text = "{ui_string}";
            if(Selection.transforms.Length > 0)
                go.transform.SetParent(Selection.transforms[0]);

            RectTransform rt;
            if((rt = go.GetComponent<RectTransform>()) != null)
            {
                rt.anchoredPosition = Vector3.zero;
            }
            
            UnityEditor.Selection.objects = new Object[] { go };
        }
    }
    #endif
}
