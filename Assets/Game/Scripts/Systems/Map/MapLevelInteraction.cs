using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using ManyTools.Variables;
using ManyTools;

/// <summary>
/// Holds interaction between map and level
/// </summary>
namespace SketchFleets.Interaction
{
    [CreateAssetMenu(fileName = CreateMenus.mapLevelInteractionFileName, menuName = CreateMenus.mapLevelInteractionMenuName,
        order = CreateMenus.mapLevelInteractionOrder)]
    public class MapLevelInteraction : ScriptableObject
    {
        #region Private Fields
        private static bool isHandlingLoad = false;
        private static Action currentOnLoad;
        #endregion

        #region Public Fields
        //Current map
        public static ConstelationMap map;
        public static ConstelationState state;

        public bool onlyShop = false;
        #endregion

        #region String Variables
        public StringVariable sceneMap;
        public StringVariable sceneLoading;
        public StringVariable sceneShop;
        public StringVariable sceneGameplay;
        public StringVariable sceneMenu;
        #endregion

        #region Methods
        /// <summary>
        /// Used to handle player click on map star
        /// </summary>
        /// <param name="clickedStar"></param>
        public void OnClickOnMapStar(int clickedStar)
        {
            //Save state
            Debug.Log("...");
            SaveMapState(map, () => { });
            Debug.Log("OnClick at: " + clickedStar);

            map.CloseAnimation(() =>
            {
                Debug.Log("closed");
                state.SetCurrentStar(clickedStar);

                //Set variables 
                map.currentLevel.Value = clickedStar;
                map.currentLevelDifficulty.Value = state.constelation.GetStar(clickedStar).Difficulty;
                map.currentSeed.Value = SketchFleets.ProfileSystem.Profile.GetData().Map.seed;

                try
                {
                    map.currentMap.Map.Value = clickedStar;
                    map.currentMap.Difficulty.Value = state.constelation.GetStar(clickedStar).Difficulty;
                }
                catch (Exception)
                {
                }

                if (state.constelation.GetStar(clickedStar).Difficulty == 0 || onlyShop)
                {
                    //Open level
                    SketchFleets.LoadingGame.SceneLoad = sceneShop.Value;
                    LoadScene(sceneLoading.Value, () => { });
                }
                else
                {
                    //Open level
                    //SketchFleets.LoadingGame.SceneLoad = "Scenes/Gameplay";
                    SketchFleets.LoadingGame.SceneLoad = sceneGameplay.Value;
                    LoadScene(sceneLoading.Value, () => { });
                }
            });
        }

        /// <summary>
        /// Return from level to map
        /// </summary>
        public void ReturnToMap()
        {

            LoadScene(sceneMap.Value, () =>
            {
                ConstelationMap.onMapLoad = () =>
                {
                    map.OpenInstantly();
                };
            });
        }

        //Clear
        public void OnGameOver(MonoBehaviour behaviour)
        {
            //Clear save data and return to menu
            SketchFleets.ProfileSystem.Profile.Data.Clear(behaviour, (data) =>
            {
                SketchFleets.LoadingGame.SceneLoad = sceneMenu.Value;
                LoadScene(sceneLoading.Value, () => { });
            });
        }

        public void SaveReturningToMenu(MonoBehaviour behaviour)
        {
            Constelation.Star star = state.constelation.GetStar(state.GetCurrentStar());

            foreach (Constelation.StarJunction j in star.toJunctions)
            {
                if (!state.IsChoosen(j.starA.Id))
                    continue;

                //Open linked starts
                state.Open(j.starA.Id);
                state.Open(j.starB.Id);
            }

            SaveMapState(behaviour, () => { });
        }

        /// <summary>
        /// Return from level to map, and open a star
        /// </summary>
        public void ReturnToMapOpeningStar()
        {
            LoadScene(sceneMap.Value, () =>
            {
                //Debug.Log("aaa");
                ConstelationMap.onMapLoad = () =>
                {
                    //Debug.Log("bbb");
                    map.OpenInstantly();

                    Time.timeScale = 1;

                    //Schedule Open Star Animation
                    map.UnlockNextLevel();

                    //Save Map
                    SaveMapState(map, () => { });
                };
            });
        }

        /// <summary>
        /// Open map from menu
        /// </summary>
        public void OpenMap(MonoBehaviour source, bool continueGame)
        {
            LoadMapState(source, continueGame, () =>
            {
                SketchFleets.LoadingGame.SceneLoad = sceneMap.Value;
                LoadScene(sceneLoading.Value, () =>
                {
                    ConstelationMap.onMapLoad = () =>
                    {
                        map.OpenAnimation(() => { });
                    };
                });
            });
        }

        /// <summary>
        /// Used to load scenes with handler
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="onLoad"></param>
        public void LoadScene(string scene, Action onLoad)
        {
            if (!isHandlingLoad)
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
        public void LoadMapState(MonoBehaviour source, bool continueGame, Action callback)
        {
            //if (state == null)
            state = new ConstelationState(null);

            //Check if there's data to be loaded
            SketchFleets.ProfileSystem.Profile.LoadProfile((save) =>
            {

                if (continueGame)
                {
                    callback();
                }
                else
                {
                    SketchFleets.ProfileSystem.Profile.Data.Clear(source, (data) =>
                    {
                        callback();
                    });
                }
            });
        }

        /// <summary>
        /// Save map state to profile
        /// </summary>
        public void SaveMapState(MonoBehaviour source, Action callback)
        {
            Debug.Log("Saving...");
            SketchFleets.ProfileSystem.Profile.SaveProfile((save) =>
            {
                Debug.Log("Map saved!");
                callback();
            });
        }

        /// <summary>
        /// Check if there's a game to continue
        /// </summary>
        /// <param name="callback"></param>
        public void HasGameToContinue(MonoBehaviour source, Action<bool> callback)
        {
            SketchFleets.ProfileSystem.Profile.LoadProfile((data) =>
            {
                Debug.Log("aa: " + SketchFleets.ProfileSystem.Profile.GetData().Map.seed);
                callback(SketchFleets.ProfileSystem.Profile.GetData().Map.seed != -1);
            });
        }
        #endregion

        #region Unity Callbacks
        /// <summary>
        /// /// Unity handler for on scene loaded
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //Call
            if (currentOnLoad != null)
                currentOnLoad();
        }
        #endregion
    }
}