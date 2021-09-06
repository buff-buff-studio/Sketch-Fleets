using System;
using System.Collections;
using System.Collections.Generic;
using ManyTools.Variables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SketchFleets
{
    public class ShootingTarget : MonoBehaviour
    {
        private PlayerControl playerControl;
        private RectTransform rectTransform;

        private float CanvasX;
        private float CanvasY;
        
        public RectTransform canvas;
        public Vector2Reference targetPos;
        
        private float sense;

        public Vector2 target;

        private void Awake()
        {
            playerControl = new PlayerControl();
            playerControl.Enable();
            
            CanvasX = canvas.rect.width;
            CanvasY = canvas.rect.height;
        }

        private void Start()
        {
            TryGetComponent(out rectTransform);
            ControlTarget(true);

            if (PlayerPrefs.GetFloat("JoystickSense") < 5)
                PlayerPrefs.SetFloat("JoystickSense", 15);

            sense = PlayerPrefs.GetFloat("JoystickSense");
        }

        void Update()
        {
            if (CanvasX != canvas.rect.width)
            {
                CanvasX = canvas.rect.width;
                CanvasY = canvas.rect.height;
            }

            ControlTarget(false);
        }

        private void ControlTarget(bool forceUpdate)
        {
            //Debug.Log("s");
            Vector2 joystickPos = playerControl.Player.Look.ReadValue<Vector2>();
            if (joystickPos != Vector2.zero || forceUpdate)
            {
                Vector2 pos = Vector2.zero;
                if (TargetX(joystickPos.x * sense))
                    pos += Vector2.right * joystickPos.x;

                if (TargetY(joystickPos.y * sense))
                    pos += Vector2.up * joystickPos.y;

                rectTransform.Translate(pos * sense, Space.World);
            }
            target = Camera.main.ViewportToWorldPoint(Camera.main.ScreenToViewportPoint(rectTransform.position));
        }

        public void SetSense()
        {
            sense = PlayerPrefs.GetFloat("JoystickSense");
        }
        
        private bool TargetX(float x)
        {
            if (rectTransform.anchoredPosition.x >= 0)
            {
                if (rectTransform.anchoredPosition.x + x <= CanvasX / 2 - rectTransform.sizeDelta.x / 2)
                    return true;
                else
                    return false;
            }
            else
            {
                if (rectTransform.anchoredPosition.x + x >= -(CanvasX / 2 - rectTransform.sizeDelta.x / 2))
                    return true;
                else
                    return false;
            }
        }
        private bool TargetY(float y)
        {
            if (rectTransform.anchoredPosition.y >= 0)
            {
                if (rectTransform.anchoredPosition.y + y <= CanvasY / 2 - rectTransform.sizeDelta.y / 2)
                    return true;
                else
                    return false;
            }
            else
            {
                if (rectTransform.anchoredPosition.y + y >= -(CanvasY / 2 - rectTransform.sizeDelta.y / 2))
                    return true;
                else
                    return false;
            }
        }
    }
}
