using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using SketchFleets.Data;
using UnityEngine.Serialization;

namespace SketchFleets
{
    public class LoadingGame : MonoBehaviour
    {
        public static string SceneLoad = "Gameplay";

        public Vector3 IconRotate;
        public Image LoadIcon;
        [FormerlySerializedAs("Difficulty")] public MapAttributes map;
        void Start()
        {
            StartCoroutine(LoadingScene());

            LoadIcon.color = map.MapColor[map.Difficulty];
        }

        IEnumerator LoadingScene()
        {
            while(SceneLoad == null)
            {
                yield return null;
            }

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneLoad);

            while (!asyncOperation.isDone)
            {
                LoadIcon.rectTransform.parent.eulerAngles += IconRotate * asyncOperation.progress * Time.deltaTime;

                yield return null;
            }
        }
    }
}