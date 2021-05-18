using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace SketchFleets
{
    public class loadingScriptTEST : MonoBehaviour
    {
        public Image loadBar;
        public TextMeshProUGUI loadText;
        void Start()
        {
            StartCoroutine(LoadingScene());
        }

        void Update()
        {
        
        }

        IEnumerator LoadingScene()
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Game");
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                //Debug.Log(asyncOperation.progress);
                loadBar.fillAmount = asyncOperation.progress;
                if (asyncOperation.progress >= 0.9f)
                {
                    loadText.text = "Press the space bar to continue";
                    loadText.fontStyle = FontStyles.Underline;
                    if (Input.GetKeyDown(KeyCode.Space))
                        asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}
