using System;
using System.Collections;
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

        private IAA_SketchFleetsInputs playerControl;

        private int closeFinger = 0;

        private int controlsMode => PlayerPrefs.GetInt("controlsMode");
        private int eventsMode => PlayerPrefs.GetInt("eventsMode");

        public DebugScript db;

        private void OnEnable()
        {
            playerControl = new IAA_SketchFleetsInputs();
            playerControl.Enable();
        }

        private void OnDisable()
        {
            playerControl.Disable();
            playerControl = null;
        }
        
        private void Start()
        {
            EnhancedTouchSupport.Enable();
            
            ControlsSet();
            if(eventsMode==0)
                commandsButtons.SetActive(false);
        }

        private void ControlsSet()
        {
            if (controlsMode != 0)
                movementJoysticks[0].SetActive(true);
            
            if(controlsMode!=1)
                movementJoysticks[controlsMode-1].SetActive(false);
        }

        private void Update()
        {
            db.UpdateDebug($"Control Mode: {controlsMode}",9);
            db.UpdateDebug($"Event Mode: {eventsMode}",8);
            db.UpdateDebug($"Touch Count: {Touch.activeTouches.Count}",7);
            db.UpdateDebug($"Pos2: {playerControl.InGame.TouchTwo.ReadValue<Vector2>()}",6);
            db.UpdateDebug($"Pos1: {playerControl.InGame.TouchOne.ReadValue<Vector2>()}",5);
            db.UpdateDebug($"Rad: {playerControl.InGame.TouchOneRadius.ReadValue<Vector2>()}",4);

            if (HUD.activeSelf && Time.timeScale == 1)
            {
                ControlsUpdate();
                if (playerControl.InGame.InputDraw.triggered)
                    OpenDraw();
                if (playerControl.InGame.ShipFire.triggered)
                    FireShip();
            }
            else
            {
                playerControl.InGame.StartDraw.started += DrawInput;
                playerControl.InGame.StartDraw.canceled += DrawInput;
            }
        }

        private void ControlsUpdate()
        {
            if (controlsMode == 0)
                TouchInput();
            else if (controlsMode == 1)
                JoystickInput();
            else
                TouchJoystickInput();
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
                    TouchTwoPos();
            }
        }

        public void SelectInput()
        {
            if (Time.timeScale != 1) return;
            
            Vector2 touch = Camera.main.ViewportToWorldPoint(Camera.main.ScreenToViewportPoint(playerControl.InGame.TouchOne.ReadValue<Vector2>()));

            float distShip = Vector2.Distance(touch, mothership.transform.position);
            float distTarget = Vector2.Distance(touch, shootingTarget.transform.position);

            if (distShip*1.25f > distTarget)
                closeFinger = 2;
            else
                closeFinger = 1;
        }

        public void TouchOnePos()
        {
            if(Time.timeScale != 1 || closeFinger == 0 || playerControl.InGame.TouchOne.ReadValue<Vector2>() == Vector2.zero) return;
            
            if(closeFinger == 1)
                mothership.Move(playerControl.InGame.TouchOne.ReadValue<Vector2>(),TouchOneRadius());
            else
                shootingTarget.ControlTarget(playerControl.InGame.TouchOne.ReadValue<Vector2>(),TouchOneRadius());
        }
        
        public void TouchTwoPos()
        {
            if(Time.timeScale != 1 || closeFinger == 0 || playerControl.InGame.TouchTwo.ReadValue<Vector2>() == Vector2.zero) return;
            
            if(closeFinger == 2)
                mothership.Move(playerControl.InGame.TouchTwo.ReadValue<Vector2>(),TouchOneRadius());
            else
                shootingTarget.ControlTarget(playerControl.InGame.TouchTwo.ReadValue<Vector2>(),TouchOneRadius());
        }
        
        private Vector2 TouchOneRadius()
        {
            if(Settings.Get<bool>("touchRay"))
                return playerControl.InGame.TouchOne.ReadValue<Vector2>();
            else
                return Vector2.one*.04f;
        }
        
        private Vector2 TouchTwoRadius()
        {
            if(Settings.Get<bool>("touchRay"))
                return playerControl.InGame.TouchOne.ReadValue<Vector2>();
            else
                return Vector2.one*.04f;
        }
        
        #endregion

        #region Touch & Joystick Input

        private void TouchJoystickInput()
        {
            if (controlsMode == 2)
            {
                JoystickTarget();
                if (playerControl.Player.Look.ReadValue<Vector2>() == Vector2.zero)
                {
                    closeFinger = 1;
                    TouchOnePos();
                }
                else
                {
                    closeFinger = 2;
                    TouchTwoPos();
                }          
            }
            else
            {
                JoystickMove();
                if (playerControl.Player.Move.ReadValue<Vector2>() == Vector2.zero)
                {
                    closeFinger = 2;
                    TouchOnePos();
                }
                else
                {
                    closeFinger = 1;
                    TouchTwoPos();
                }
            }

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
            shootingTarget.JoystickControlTarget(playerControl.Player.Look.ReadValue<Vector2>());
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

        public void DrawInput(InputAction.CallbackContext context)
        {
            _lineDrawer.DrawCallBack(context);
        }
        
        public void FireShip()
        {
            if (Touch.activeTouches.Count != 1 && !HUD.activeSelf) return;
                _cyanPathDrawer.CyanGO();
        }

        #endregion
    }
}