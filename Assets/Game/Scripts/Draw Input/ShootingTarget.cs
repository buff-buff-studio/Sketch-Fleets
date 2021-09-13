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
        
        #endregion

        #region Public Fields

        public Transform mothershipTransform;
        
        public Transform targetPoint;
        
        public Vector2Reference targetPos;
        
        public Vector2 target;

        #endregion

        public Text t;

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
            ControlTarget(true);
        }

        private void Update()
        {
            ControlTarget(false);
            
            Debug.DrawLine(targetPoint.position, GetMothershipPosition(), Color.blue);
            Vector2 vec = PlayerControl.Player.MoveRadius.ReadValue<Vector2>();
            t.text = ((vec.x + vec.y) / 2).ToString();
        }

        #endregion

        private void ControlTarget(bool forceUpdate)
        {
            Look(GetMothershipPosition());
            
            Vector2 joystickPos = PlayerControl.Player.Look.ReadValue<Vector2>();
            target = targetPoint.position;
            
            if (joystickPos == Vector2.zero || !forceUpdate) return;
            targetTransform.position = GetTargetPosition();
        }

        #region Private Methods

        /// <summary>
        ///     Sets the target position
        /// </summary>
        /// <returns>The</returns>
        private Vector2 GetTargetPosition()
        {
            return mainCameraCache.ViewportToWorldPoint(mainCameraCache.ScreenToViewportPoint(targetTransform.position));
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