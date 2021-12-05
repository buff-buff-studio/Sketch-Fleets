using UnityEngine;

namespace SketchFleets.Systems.DrawInput
{
    /// <summary>
    /// A class that applies a requirement to drawing a line.
    /// </summary>
    public sealed class DrawingRequirementApplier : MonoBehaviour
    {
        #region Private Fields

        [Header("Drawing Requirements")]
        [SerializeField]
        private int requiredShapeIndex;

        [SerializeField]
        private bool forceEnterDrawMode;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            LineDrawer drawer = FindObjectOfType<LineDrawer>(true);

            if (forceEnterDrawMode)
            {
                Debug.Log("Forcing drawing to begin");
                drawer.ForceBeginDraw();
            }
            
            drawer.RequireOutcome(requiredShapeIndex);
        }

        #endregion
    }
}