using System;
using SketchFleets.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Serialization;
using SketchFleets.SettingsSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace SketchFleets
{
    public sealed class MasterController : MonoBehaviour
    {
        [SerializeField]
        private PauseScript pauseScript;
        [SerializeField]
        private ShootingTarget shootingTarget;
        [SerializeField]
        private Mothership mothership;
        
        [FormerlySerializedAs("cyanShoot")]
        [SerializeField]
        private CyanPathDrawer _cyanPathDrawer;

        private IAA_PlayerControl playerControl;

        private int closeFinger = 0;

        public DebugScript db;

        private void Awake()
        {
            playerControl = new IAA_PlayerControl();
            playerControl.Enable();
            EnhancedTouchSupport.Enable();
        }

        private void Update()
        {
            db.UpdateDebug("Rad2: " + playerControl.Player.TouchTwoRadius.ReadValue<Vector2>().ToString("0.0000"),8);
            db.UpdateDebug("Pos2: " + playerControl.Player.TouchTwo.ReadValue<Vector2>().ToString(),7);
            db.UpdateDebug("Rad1: " + playerControl.Player.TouchOneRadius.ReadValue<Vector2>().ToString("0.0000"),5); 
            db.UpdateDebug("Pos1: " + playerControl.Player.TouchOne.ReadValue<Vector2>().ToString(),4);

            if (Touch.activeTouches.Count == 0)
                closeFinger = 0;
            else if (closeFinger == 0)
                SelectInput();
            else
            {
                TouchOnePos();
                if (Touch.activeTouches.Count > 1)
                    TouchTwoePos();
            }
        }

        public void SelectInput()
        {
            if (Time.timeScale != 1) return;
            
            Vector2 touch = Camera.main.ViewportToWorldPoint(Camera.main.ScreenToViewportPoint(playerControl.Player.TouchOne.ReadValue<Vector2>()));

            float distShip = Vector2.Distance(touch, mothership.transform.position);
            float distTarget = Vector2.Distance(touch, shootingTarget.transform.position);

            if (distShip*1.25f > distTarget)
                closeFinger = 2;
            else
                closeFinger = 1;
        }

        public void TouchOnePos()
        {
            if(Time.timeScale != 1 || closeFinger == 0) return;
            
            if(closeFinger == 1)
                mothership.Move(playerControl.Player.TouchOne.ReadValue<Vector2>(),TouchOneRadius());
            else
                shootingTarget.ControlTarget(playerControl.Player.TouchOne.ReadValue<Vector2>(),TouchOneRadius());
        }
        
        public void TouchTwoePos()
        {
            if(Time.timeScale != 1 || closeFinger == 0) return;
            
            if(closeFinger == 2)
                mothership.Move(playerControl.Player.TouchTwo.ReadValue<Vector2>(),TwoRad());
            else
                shootingTarget.ControlTarget(playerControl.Player.TouchTwo.ReadValue<Vector2>(),TwoRad());
        }

        private Vector2 TouchOneRadius()
        {
            if(Settings.Get<bool>("touchRay"))
                return playerControl.Player.TouchTwoRadius.ReadValue<Vector2>();
            else
                return Vector2.one*.04f;
        }
        
        private Vector2 TwoRad()
        {
            if(Settings.Get<bool>("touchRay"))
                return playerControl.Player.TouchTwoRadius.ReadValue<Vector2>();
            else
                return Vector2.one*.04f;
        }
    }
}