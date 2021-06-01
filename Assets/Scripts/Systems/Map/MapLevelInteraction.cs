using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Holds interaction between map and level
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
    public static ConstelationState state;
    #endregion

    #region Methods
    /// <summary>
    /// Used to handle player click on map star
    /// </summary>
    /// <param name="clickedStar"></param>
    public static void OnClickOnMapStar(int clickedStar)
    {   
        //Save state
        SaveMapState(map,() => {});

        map.CloseAnimation(() => {
            state.SetCurrentStar(clickedStar);
            
            //Set variables 
            map.currentLevel.Value = clickedStar;
            map.currentLevelDifficulty.Value = state.constelation.GetStar(clickedStar).Difficulty;
            map.currentSeed.Value = state.seed;

            try
            {
                map.currentMap.Map.Value = clickedStar;
                map.currentMap.Difficulty.Value = state.constelation.GetStar(clickedStar).Difficulty;
            }
            catch(Exception)
            {             
            }
            //Open level
            LoadScene("Scenes/Loading",() => {});
        });
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

    public static void SaveReturningToMenu(MonoBehaviour behaviour)
    {
        Constelation.Star star = state.constelation.GetStar(state.GetCurrentStar());

         foreach(Constelation.StarJunction j in star.toJunctions)
        {
            if(!state.IsChoosen(j.starA.Id))
                    continue;

            //Open linked starts
            state.Open(j.starA.Id);
            state.Open(j.starB.Id);
        }

        SaveMapState(behaviour,() => {});
    }

    /// <summary>
    /// Return from level to map, and open a star
    /// </summary>
    public static void ReturnToMapOpeningStar()
    {
        LoadScene("Scenes/Map",() => {
            Debug.Log("aaa");
            ConstelationMap.onMapLoad = () => {
                Debug.Log("bbb");
                map.OpenInstantly();

                //Schedule Open Star Animation
                map.UnlockNextLevel();

                //Save Map
                SaveMapState(map,() => {});
            };
        });
    }

    /// <summary>
    /// Open map from menu
    /// </summary>
    public static void OpenMap(MonoBehaviour source,bool continueGame)
    {
        LoadMapState(source,continueGame,() => {
            LoadScene("Scenes/Map",() => {
                ConstelationMap.onMapLoad = () => {
                    map.OpenAnimation(() => {});
                };
            });
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

    /// <summary>
    /// Load map state from profile
    /// </summary>
    /// <param name="source"></param>
    /// <param name="callback"></param>
    public static void LoadMapState(MonoBehaviour source,bool continueGame,Action callback)
    {
        if(state == null)
            state = new ConstelationState(null);
            
        //Check if there's data to be loaded
        Debug.Log(continueGame ? "Loading..." : "Creating new game...");
        SketchFleets.ProfileSystem.Profile.Using(source);
        SketchFleets.ProfileSystem.Profile.LoadProfile((save) => {

            if(continueGame)
            {
                if(SketchFleets.ProfileSystem.Profile.GetData().save.HasKey("mapState"))
                {
                    byte[] bytes = SketchFleets.ProfileSystem.Profile.GetData().save.Get<byte[]>("mapState");       
                    state.LoadData(bytes);

                    Debug.Log("Map Loaded!");
                    callback();   
                }
            }
            else
            {
                SketchFleets.ProfileSystem.Profile.GetData().save.Remove("mapState");
                SaveMapState(source,callback);
            }     
        });  
    }

    /// <summary>
    /// Save map state to profile
    /// </summary>
    public static void SaveMapState(MonoBehaviour source,Action callback)
    {
        Debug.Log("Saving...");
        SketchFleets.ProfileSystem.Profile.Using(source);
        SketchFleets.ProfileSystem.Profile.GetData().save["mapState"] = state.ToData();
        SketchFleets.ProfileSystem.Profile.SaveProfile((save) => {
            Debug.Log("Map saved!");
            callback();
        });
    }
    
    /// <summary>
    /// Check if there's a game to continue
    /// </summary>
    /// <param name="callback"></param>
    public static void HasGameToContinue(MonoBehaviour source,Action<bool> callback)
    {   
        SketchFleets.ProfileSystem.Profile.Using(source);
        SketchFleets.ProfileSystem.Profile.LoadProfile((data) => {
            callback(data.save.HasKey("mapState"));
        });
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