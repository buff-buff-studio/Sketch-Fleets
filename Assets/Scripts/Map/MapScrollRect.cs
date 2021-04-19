using UnityEngine.UI;

/// <summary>
/// ScrollRect with custom method to instantly update bounds
/// </summary>
public class MapScrollRect : ScrollRect
{
    #region Public Methods
    /// <summary>
    /// Auto update content.anchoredPosition value, keeping anchored position inside bounds
    /// </summary>
    public void PublicUpdateBounds()
    {
        LateUpdate();
    } 
    #endregion
}