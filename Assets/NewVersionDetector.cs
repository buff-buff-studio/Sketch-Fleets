using System;
using System.Collections;
using System.Collections.Generic;
using ManyTools.UnityExtended;
using UnityEngine;

namespace SketchFleets
{
    public class NewVersionDetector : MonoBehaviour
    {
        [SerializeField] private WindowManager windowManager;

        private void Awake()
        {
            if (PlayerPrefs.GetString("gameVersion") != Application.version.ToString())
            {
                PlayerPrefs.SetString("gameVersion", Application.version.ToString());
                windowManager.SwitchToMenu("Update");
            }
        }
    }
}
