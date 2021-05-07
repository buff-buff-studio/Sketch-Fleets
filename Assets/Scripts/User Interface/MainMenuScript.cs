using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    #region Public Methods

    public void LoadNewGame()
    {
        MapLevelInteraction.OpenMap();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}
