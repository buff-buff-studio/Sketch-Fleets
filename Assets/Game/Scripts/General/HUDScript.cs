using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManyTools.Variables;
using UnityEngine.SceneManagement;
using SketchFleets.Interaction;

public class HUDScript : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private FloatReference life;
    private float lifeFull;
    #endregion

    #region Public Fields
    public MapLevelInteraction interaction;
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
        interaction.OnGameOver(this);
    }

    public void Menu()
    {
        Time.timeScale = 1;
        life.Value = lifeFull;
        SceneManager.LoadScene("Menu");
        interaction.SaveReturningToMenu(this);
    }
    #endregion
}
