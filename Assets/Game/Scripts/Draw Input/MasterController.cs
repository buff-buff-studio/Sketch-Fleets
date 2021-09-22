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
        private ShootingTarget shootingTarget;
        [SerializeField]
        private Mothership mothership;
        [FormerlySerializedAs("cyanShoot")]
        [SerializeField]
        private CyanPathDrawer _cyanPathDrawer;
        [SerializeField]
        private LineDrawer _lineDrawer;
        
        [SerializeField]
        private GameObject HUD;

        [SerializeField] 
        private GameObject[] movementJoysticks;
        
        [SerializeField] 
        private GameObject commandsButtons;

        private IAA_PlayerControl playerControl;

        private int closeFinger = 0;

        private int controlsMode => PlayerPrefs.GetInt("controlsMode");
        private int eventsMode => PlayerPrefs.GetInt("eventsMode");

        public DebugScript db;

        private void Start()
        {
            playerControl = new IAA_PlayerControl();
            playerControl.Enable();
            EnhancedTouchSupport.Enable();
            
            ControlsSet();
            if(eventsMode==0)
                commandsButtons.SetActive(false);
        }

        private void ControlsSet()
        {
            if (controlsMode == 1)
            {
                movementJoysticks[0].SetActive(true);
                movementJoysticks[1].SetActive(false);
            }
            else if(controlsMode==2)
            {
                movementJoysticks[0].SetActive(true);
            }
        }

        private void Update()
        {
            db.UpdateDebug($"{controlsMode}",9);
            db.UpdateDebug($"{eventsMode}",8);
            db.UpdateDebug($"{playerControl.Player.Target.ReadValue<Vector2>()}",7);
            
            if (HUD.activeSelf && Time.timeScale == 1);
                ControlsUpdate();
        }

        private void ControlsUpdate()
        {
            if (controlsMode == 0)
                TouchInput();
            else if (controlsMode == 1)
                TouchJoystickInput();
            else
                JoystickInput();
        }

        #region Touch Input
        
        private void TouchInput()
        {
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
                mothership.Move(playerControl.Player.TouchTwo.ReadValue<Vector2>(),TouchOneRadius());
            else
                shootingTarget.ControlTarget(playerControl.Player.TouchTwo.ReadValue<Vector2>(),TouchOneRadius());
        }
        
        private Vector2 TouchOneRadius()
        {
            if(Settings.Get<bool>("touchRay"))
                return playerControl.Player.TouchOne.ReadValue<Vector2>();
            else
                return Vector2.one*.04f;
        }
        
        private Vector2 TouchTwoRadius()
        {
            if(Settings.Get<bool>("touchRay"))
                return playerControl.Player.TouchOne.ReadValue<Vector2>();
            else
                return Vector2.one*.04f;
        }
        
        #endregion

        #region Touch & Joystick Input

        private void TouchJoystickInput()
        {
            closeFinger = 1;
            TouchOnePos();
            JoystickTarget();
        }

        #endregion

        #region Joystick Input

        private void JoystickInput()
        {
            JoystickMove();
            JoystickTarget();
        }
        
        private void JoystickMove()
        {
            mothership.JoystickMove(playerControl.Player.Move.ReadValue<Vector2>());
        }
        
        private void JoystickTarget()
        {
            shootingTarget.JoystickControlTarget(playerControl.Player.Target.ReadValue<Vector2>());
        }

        #endregion

        #region Events Controls

        public void OpenDraw()
        {
            if (Touch.activeTouches.Count != 2 && !HUD.activeSelf) return;
            HUD.SetActive(false);
            _lineDrawer.gameObject.SetActive(true);
            _lineDrawer.BulletTime(.5f);
        }

        public void FireShip()
        {
            if (Touch.activeTouches.Count != 1 && !HUD.activeSelf) return;
                _cyanPathDrawer.CyanGO();
        }

        #endregion
    }
}