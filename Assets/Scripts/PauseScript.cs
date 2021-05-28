using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets
{
    public class PauseScript : MonoBehaviour
    {
        public GameObject PauseMenu;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
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
        }

        public void Return()
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
