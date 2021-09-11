using ManyTools.Variables;
using UnityEngine;
using UnityEngine.Serialization;

namespace SketchFleets
{
    public sealed class ShootingTarget : MonoBehaviour
    {
        private PlayerControl playerControl;
        private RectTransform rectTransform;

        private float CanvasX;
        private float CanvasY;

        private float sense;
        
        private Rect canvasRect;

        private Camera mainCameraCache;
        
        [FormerlySerializedAs("canvas")]
        public RectTransform canvasRectTransform;
        
        public Vector2Reference targetPos;

        public float XSense;
        public Vector2 target;

        #region Unity Callbacks

        private void Awake()
        {
            mainCameraCache = Camera.main;
            playerControl = new PlayerControl();
            playerControl.Enable();

            canvasRect = canvasRectTransform.rect;
            CanvasX = canvasRect.width;
            CanvasY = canvasRect.height;
        }

        private void Start()
        {
            TryGetComponent(out rectTransform);
            ControlTarget(true);

            if (PlayerPrefs.GetFloat("JoystickSense") < 10)
            {
                PlayerPrefs.SetFloat("JoystickSense", 15);
            }

            sense = PlayerPrefs.GetFloat("JoystickSense");
        }

        private void Update()
        {
            if (!Mathf.Approximately(CanvasX, canvasRect.width))
            {
                CanvasX = canvasRect.width;
                CanvasY = canvasRect.height;
            }

            ControlTarget(false);
        }

        #endregion

        private void ControlTarget(bool forceUpdate)
        {
            Vector2 joystickPos = playerControl.Player.Look.ReadValue<Vector2>();

            if (joystickPos != Vector2.zero || forceUpdate)
            {
                Vector2 pos = Vector2.zero;

                if (TargetX(joystickPos.x * XSense * sense))
                {
                    pos += Vector2.right * (joystickPos.x * XSense);
                }

                if (TargetY(joystickPos.y * sense))
                {
                    pos += Vector2.up * joystickPos.y;
                }

                rectTransform.Translate(pos * sense, Space.World);
            }

            target = GetTargetPosition();
        }

        #region Private Methods

        /// <summary>
        ///     Sets the target position
        /// </summary>
        /// <returns>The</returns>
        private Vector2 GetTargetPosition()
        {
            return mainCameraCache.ViewportToWorldPoint(mainCameraCache.ScreenToViewportPoint(rectTransform.position));
        }

        /// <summary>
        ///     
        /// </summary>
        public void SetSense()
        {
            sense = PlayerPrefs.GetFloat("JoystickSense");
        }

        private bool TargetX(float x)
        {
            if (rectTransform.anchoredPosition.x >= 0)
            {
                return rectTransform.anchoredPosition.x + x <= CanvasX / 2 - rectTransform.sizeDelta.x / 2;
            }

            return rectTransform.anchoredPosition.x + x >= -(CanvasX / 2 - rectTransform.sizeDelta.x / 2);
        }

        private bool TargetY(float y)
        {
            if (rectTransform.anchoredPosition.y >= 0)
            {
                return rectTransform.anchoredPosition.y + y <= CanvasY / 2 - rectTransform.sizeDelta.y / 2;
            }

            return rectTransform.anchoredPosition.y + y >= -(CanvasY / 2 - rectTransform.sizeDelta.y / 2);
        }

        #endregion
    }
}