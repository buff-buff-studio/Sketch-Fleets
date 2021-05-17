using ManyTools.UnityExtended;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    #region Unity Callbacks

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    #endregion

    #region Public Methods

    /// <summary>
    /// Loads a given scene
    /// </summary>
    /// <param name="sceneName">The build name of the scene</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Quits the game application
    /// </summary>
    public void QuitApplication()
    {
        Application.Quit();
    }
    
    #endregion
}
