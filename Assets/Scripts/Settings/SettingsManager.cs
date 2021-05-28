using UnityEngine;

namespace SketchFleets.SettingsSystem
{   
    /// <summary>
    /// You can put this in a DontDestroyObjectOnLoad to auto load settings
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        void Start()
        {
            Settings.Load(this,() => {});
        }
    }
}
