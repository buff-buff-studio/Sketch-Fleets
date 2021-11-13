using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace SketchFleets
{
    public class PauseScript : MonoBehaviour
    {
        public GameObject PauseMenu;

        void Update()
        {

            try
            {
                SketchFleets.General.LevelManager.Instance.PauseShellCount.text =
                    ProfileSystem.Profile.Data.Coins.ToString();
            }
            catch
            {
                SceneManager.LoadScene("Menu");
            }
        }

        public void PauseVoid(bool pause)
        {
            PauseMenu.SetActive(pause);
            if (pause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
}
