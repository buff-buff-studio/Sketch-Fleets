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
        public Vector3 IconRotate;
        public Image LoadIcon;
        public DifficultyAttributes Difficulty;
        void Start()
        {
            StartCoroutine(LoadingScene());

            LoadIcon.color = Difficulty.MapColor[Difficulty.Difficulty];
        }

        IEnumerator LoadingScene()
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Game");

            while (!asyncOperation.isDone)
            {
                LoadIcon.rectTransform.parent.eulerAngles += IconRotate * asyncOperation.progress * Time.deltaTime;

                yield return null;
            }
        }
    }
}