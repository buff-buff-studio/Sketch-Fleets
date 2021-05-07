using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Holds interation between map and level
/// </summary>
public class MapLevelInteraction : MonoBehaviour
{
    #region Private Fields
    private static bool isHandlingLoad = false;
    private static Action currentOnLoad;
    #endregion
    
    #region Public Fields
    //Current map
    public static ConstelationMap map;
    #endregion

    #region Methods
    /// <summary>
    /// Used to handle player click on map star
    /// </summary>
    /// <param name="clickedStar"></param>
    public static void OnClickOnMapStar(int clickedStar)
    {
        map.CloseAnimation(() => {
            map.constelationState.SetCurrentStar(clickedStar);
            
            //Open level
            LoadScene("Scenes/Game",() => {});
        });

        //map.UnlockNextLevel(clickedStar);
    }
    
    /// <summary>
    /// Return from level to map
    /// </summary>
    public static void ReturnToMap()
    {
        LoadScene("Scenes/Map",() => {
            ConstelationMap.onMapLoad = () => {
                map.OpenInstantly();
            };
        });
    }

    /// <summary>
    /// Return from level to map, and open a star
    /// </summary>
    public static void ReturnToMapOpeningStar()
    {
        LoadScene("Scenes/Map",() => {
            ConstelationMap.onMapLoad = () => {
                map.OpenInstantly();

                //Schedule Open Star Animation
                map.UnlockNextLevel();
            };
        });

        
    }

    /// <summary>
    /// Open map from menu
    /// </summary>
    public static void OpenMap()
    {
        LoadScene("Scenes/Map",() => {
            ConstelationMap.onMapLoad = () => {
                map.OpenAnimation(() => {});
            };
        });
    }
    
    /// <summary>
    /// Used to load scenes with handler
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="onLoad"></param>
    public static void LoadScene(string scene,Action onLoad)
    {
        if(!isHandlingLoad)
        {
            isHandlingLoad = true;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //Add handler
        currentOnLoad = onLoad;
        //Load scene
        SceneManager.LoadScene(scene); 
    }

    //To-Do
    public static void LoadMapState()
    {

    }

    //To-Do
    public static void SaveMapState()
    {

    }
    #endregion

    #region Unity Callbacks
    /// <summary>
    /// Unity handler for on scene loaded
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Call
        if(currentOnLoad != null)
            currentOnLoad();
    }
    #endregion
}