using ManyTools.Variables;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SketchFleets
{
    public sealed class ShootingTarget : MonoBehaviour
    {
        #region Private Fields
        
        private IAA_PlayerControl PlayerControl;
        
        private Transform targetTransform;

        private Camera mainCameraCache;

        private float radiusSpeed = 2.78f;
        
        #endregion

        #region Public Fields

        public Transform mothershipTransform;
        
        public Transform targetPoint;
        
        public Vector2Reference targetPos;

        public float radiusMultiply;

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            mainCameraCache = Camera.main;
            PlayerControl = new IAA_PlayerControl();
            PlayerControl.Enable();
        }

        private void Start()
        {
            TryGetComponent(out targetTransform);
        }

        #endregion

        public void ControlTarget(Vector2 targetPos, Vector2 targetRad)
        {
            targetTransform.position = GetTargetPosition(targetPos);

            targetPoint.localPosition = Vector2.MoveTowards(targetPoint.localPosition, GetRadiusPosition(targetRad), radiusSpeed*Time.deltaTime);
        }

        public void TargetLook()
        {
            Look(GetMothershipPosition());
        }

        #region Private Methods

        /// <summary>
        ///     Sets the target position
        /// </summary>
        /// <returns>The</returns>
        private Vector2 GetTargetPosition(Vector2 targetPos)
        {
            return mainCameraCache.ViewportToWorldPoint(mainCameraCache.ScreenToViewportPoint(targetPos));
        }

        private Vector2 GetRadiusPosition(Vector2 targetRad)
        {
            float radius = (float)System.Math.Round(Mathf.Lerp(targetRad.x,targetRad.y,.5f),3);
            return new Vector2(0, 1.5f + radius * radiusMultiply);
        }
        
        private Vector2 GetMothershipPosition()
        {
            return mothershipTransform.position;
        }
        
        private void Look(Vector2 target)
        {
            // Workaround, this should be done using a proper Pausing interface
            if (Mathf.Approximately(0f, Time.timeScale)) return;

            // This doesn't really solve the problem. The transform should be a member variable
            // to avoid the constant marshalling
            Transform transformCache = transform;
            transformCache.up = (Vector3)target - transformCache.position;
        }

        #endregion
    }
}