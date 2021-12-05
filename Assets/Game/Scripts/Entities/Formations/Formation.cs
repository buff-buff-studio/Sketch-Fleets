using System.Linq;
using UnityEngine;

namespace SketchFleets.Entities
{
    /// <summary>
    /// A class that holds points in a formation
    /// </summary>
    public sealed class Formation : MonoBehaviour
    {
        #region Private Fields

        [Header("Formation")]
        [SerializeField]
        private bool showFormationCenter;

        [SerializeField]
        private Transform[] formationPoints;

        #endregion

        #region Properties

        public Transform[] FormationPoints => formationPoints;

        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            GetFormationPoints();
        }

        private void OnDrawGizmos()
        {
            if (!showFormationCenter) return;
            DrawCenterPointDebugDisplay();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the center point of the formation
        /// </summary>
        /// <returns>The center point of the formation</returns>
        public Vector3 GetCenterPoint()
        {
            if (formationPoints.Contains(null))
            {
                Debug.LogError("The Formation cannot contain any null points!");
                return Vector3.zero;
            }

            float avgXCoord = formationPoints.Average(formationPoint => formationPoint.position.x);
            float avgYCoord = formationPoints.Average(formationPoint => formationPoint.position.y);
            float avgZCoord = formationPoints.Average(formationPoint => formationPoint.position.z);

            return new Vector3(avgXCoord, avgYCoord, avgZCoord);
        }
        
        /// <summary>
        /// Gets the bounds (min and max Y positions of the formation)
        /// </summary>
        /// <returns>The min and max Y positions of the formation</returns>
        public Vector2 GetBounds()
        {
            return new Vector2(GetMinY(), GetMaxY());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the maximum Y of the formation
        /// </summary>
        /// <returns>The maximum Y of the formation</returns>
        private float GetMaxY()
        {
            return formationPoints.Max(formationPoint => formationPoint.position.y);
        }

        /// <summary>
        /// Gets the minimum Y of the formation
        /// </summary>
        /// <returns>The minimum Y of the formation</returns>
        private float GetMinY()
        {
            return formationPoints.Min(formationPoint => formationPoint.position.y);
        }
        
        /// <summary>
        /// Draws a Gizmo display of the center point
        /// </summary>
        private void DrawCenterPointDebugDisplay()
        {
            Vector3 centerPoint = GetCenterPoint();
            Gizmos.color = Color.green;

            foreach (Transform formationPoint in formationPoints)
            {
                Gizmos.DrawLine(formationPoint.position, centerPoint);
            }

            Gizmos.DrawSphere(centerPoint, 0.5f);
        }

        /// <summary>
        /// Gets all points in the formation
        /// </summary>
        [ContextMenu("Get Formation Points")]
        private void GetFormationPoints()
        {
            formationPoints = GetComponentsInChildren<Transform>()
                .Where(transformComponent => transformComponent != transform)
                .ToArray();
        }

        #endregion
    }
}