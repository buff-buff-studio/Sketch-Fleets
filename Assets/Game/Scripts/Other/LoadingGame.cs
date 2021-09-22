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
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Gameplay");

            while (!asyncOperation.isDone)
            {
                LoadIcon.rectTransform.parent.eulerAngles += IconRotate * asyncOperation.progress * Time.deltaTime;

                yield return null;
            }
        }
    }
}