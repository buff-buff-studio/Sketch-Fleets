using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using SketchFleets.Data;

namespace SketchFleets
{
    public class LoadingGame : MonoBehaviour
    {
        public Image LoadBar;
        public Image LoadBarBack;
        public TextMeshProUGUI LoadText;
        public DifficultyAttributes Difficulty;
        void Start()
        {
            StartCoroutine(LoadingScene());

            LoadBar.color = Difficulty.MapColor[Difficulty.Difficulty];

            if (Difficulty.Difficulty != 4)
                return;
        }

        IEnumerator LoadingScene()
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Game");
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                //Debug.Log(asyncOperation.progress);
                LoadBar.fillAmount = asyncOperation.progress;
                if (asyncOperation.progress >= 0.9f)
                {
                    LoadText.text = "Press the space bar to continue";
                    LoadText.fontStyle = FontStyles.Underline;
                    LoadBar.fillAmount = 1;
                    if (Input.GetKeyDown(KeyCode.Space))
                        asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}