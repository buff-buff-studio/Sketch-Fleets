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
            string storedGameVersion = PlayerPrefs.GetString("gameVersion");
            string currentApplicationVersion = Application.version.ToString();

            if (storedGameVersion != currentApplicationVersion)
            {
                PlayerPrefs.SetString("gameVersion", currentApplicationVersion);
                PlayerPrefs.Save();
                windowManager.OverlayMenu("UpdateNotes");
            }
        }
    }
}
