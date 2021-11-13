using UnityEngine;
using UnityEngine.UI;
using SketchFleets.Interaction;

public sealed class MainMenuScript : MonoBehaviour
{   
    #region Public Fields
    public Button buttonContinue;
    public MapLevelInteraction interaction;
    #endregion

    #region Unity Callbacks
    /// <summary>
    /// Init main menu
    /// </summary>
    private void Start() 
    {
        buttonContinue.interactable = false;
        interaction.HasGameToContinue(this,(has) => {
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
            interaction.OpenMap(this,false);
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
            interaction.OpenMap(this,false);
        }
    }

    /// <summary>
    /// Continue game
    /// </summary>
    public void ContinueGame()
    {
        interaction.OpenMap(this,true);
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
