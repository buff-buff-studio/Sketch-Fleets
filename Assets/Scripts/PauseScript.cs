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
        private PlayerControl playerControl;

        public GameObject PauseMenu;

        private void Awake()
        {
            playerControl = new PlayerControl();
            playerControl.Enable();
        }

        void Update()
        {
            playerControl.Player.Pause.performed += PauseCall;

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

        public void PauseCall(InputAction.CallbackContext context)
        {
            if (Time.timeScale == 1)
            {
                PauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                PauseMenu.SetActive(false);
                Time.timeScale = 1;
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
