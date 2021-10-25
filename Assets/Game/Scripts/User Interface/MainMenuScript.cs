using UnityEngine;
using UnityEngine.UI;


public class MainMenuScript : MonoBehaviour
{   
    #region Public Fields
    public Button buttonContinue;
    #endregion

    #region Unity Callbacks
    /// <summary>
    /// Init main menu
    /// </summary>
    private void Start() 
    {
        buttonContinue.interactable = false;
        MapLevelInteraction.HasGameToContinue(this,(has) => {
            buttonContinue.interactable = has;
        });
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Create new game
    /// </summary>
    public void LoadNewGame()
    {
        try
        {
            MapLevelInteraction.OpenMap(this,false);
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
            MapLevelInteraction.OpenMap(this,false);
        }
    }

    /// <summary>
    /// Continue game
    /// </summary>
    public void ContinueGame()
    {
        MapLevelInteraction.OpenMap(this,true);
    }

    /// <summary>
    /// Quit game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
