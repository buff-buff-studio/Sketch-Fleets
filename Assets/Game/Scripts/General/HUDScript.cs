using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManyTools.Variables;
using UnityEngine.SceneManagement;

public class HUDScript : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private FloatReference life;
    private float lifeFull;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        lifeFull = life;
    }
    #endregion

    #region Die Buttons
    public void Replay()
    {
        Time.timeScale = 1;
        life.Value = lifeFull;
        SceneManager.LoadScene("Game");
    }

    public void GameOver()
    {
        Time.timeScale = 1;
        life.Value = lifeFull;
        SceneManager.LoadScene("Menu");
        MapLevelInteraction.OnGameOver(this);
    }

    public void Menu()
    {
        Time.timeScale = 1;
        life.Value = lifeFull;
        SceneManager.LoadScene("Menu");
        MapLevelInteraction.SaveReturningToMenu(this);
    }
    #endregion
}
