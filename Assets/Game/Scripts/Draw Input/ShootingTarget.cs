using ManyTools.Variables;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SketchFleets
{
    public sealed class ShootingTarget : MonoBehaviour
    {
        #region Private Fields

        private Transform targetTransform;

        private Camera mainCameraCache;

        private float radiusSpeed = 2.78f;

        private float CanvasX;
        private float CanvasY;

        private float sense;
        
        private Rect canvasRect;

        #endregion

        #region Public Fields

        public Transform mothershipTransform;
        
        public Transform targetPoint;
        
        public RectTransform canvasRectTransform;

        public float radiusMultiply;

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            mainCameraCache = Camera.main;

            canvasRect = canvasRectTransform.rect;
            CanvasX = canvasRect.width;
            CanvasY = canvasRect.height;
        }

        private void Start()
        {
            TryGetComponent(out targetTransform);

            if (PlayerPrefs.GetFloat("JoystickSense") ==0 || PlayerPrefs.GetFloat("JoystickSense")>1)
                PlayerPrefs.SetFloat("JoystickSense", 1);

            sense = PlayerPrefs.GetFloat("JoystickSense");
        }

        #endregion

        #region Touch Input
        
        public void ControlTarget(Vector2 targetPos, Vector2 targetRad)
        {
            targetTransform.position = GetTargetPosition(targetPos);

            targetPoint.localPosition = Vector2.MoveTowards(targetPoint.localPosition, GetRadiusPosition(targetRad), radiusSpeed*Time.deltaTime);
            
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
        
        #endregion

        #region Joystick Input
        
        public void JoystickControlTarget(Vector2 joystickPos)
        {
            targetPoint.localPosition = Vector2.zero;
            
            if (joystickPos != Vector2.zero)
            {
                Vector2 pos = Vector2.zero;

                if(TargetX((joystickPos.x * XSense)/2))
                    pos += Vector2.right * ((joystickPos.x * XSense)/2);
                if(TargetY((joystickPos.y * XSense)/2))
                    pos += Vector2.up * ((joystickPos.y * sense)/2);

                targetTransform.Translate(pos, Space.World);
            }
        }

        private float XSense => sense * (mainCameraCache.aspect/2);
        
        private Vector2 GetTargetPosInCanvas=> mainCameraCache.ViewportToScreenPoint(mainCameraCache.WorldToViewportPoint(targetTransform.position));

        private bool TargetX(float x)
        {
            if (x < 0)
                return GetTargetPosInCanvas.x - x > 10;
            else
                return GetTargetPosInCanvas.x + x < CanvasX-10;
        }

        private bool TargetY(float y)
        {
            if (y < 0)
                return GetTargetPosInCanvas.y - y > 10;
            else
                return GetTargetPosInCanvas.y + y < CanvasY-10;
        }

        #endregion
        
    }
}