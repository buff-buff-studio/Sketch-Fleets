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
        private ColorsInventory _colorsInventory;

        [SerializeField]
        private GameObject HUD;
        [SerializeField]
        private GameObject InventoryHUD;
        [SerializeField]
        private GameObject ColorHUD;

        [SerializeField]
        private RectTransform JoystickL;
        [SerializeField]
        private RectTransform JoystickR;

        [SerializeField]
        private GameObject fireShipButton;

        private IAA_SketchFleetsInputs playerControl;

        private int closeFinger = 0;

        private int controlsMode => Settings.GetObject().controlMode;

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
        }

        private void ControlsSet()
        {
            if (controlsMode == 0)
            {
                JoystickL.gameObject.SetActive(false);
                JoystickR.gameObject.SetActive(false);
                fireShipButton.SetActive(false);
            }
            else if (controlsMode == 2)
            {
                JoystickL.gameObject.SetActive(false);

            }
            else if (controlsMode == 3)
            {
                JoystickR.gameObject.SetActive(false);
                fireShipButton.SetActive(false);
            }
        }

        private void Update()
        {
            UpdateDebug();

            if (HUD.activeSelf && !Mathf.Approximately(Time.timeScale, 0f))
            {
                ControlsUpdate();
                if (playerControl.InGame.InputDraw.triggered)
                    OpenDraw();
            }
            else
            {
                playerControl.InGame.StartDraw.started += DrawInput;
                playerControl.InGame.StartDraw.canceled += DrawInput;
            }
        }

        private void UpdateDebug()
        {
            if (PlayerPrefs.GetInt("debugMode") == 0) return;
            db.UpdateDebug($"JRight: {Vector2.Distance(TouchOne, JoystickRightPos) < JoystickR.sizeDelta.x * .75f}", 18);
            db.UpdateDebug($"JLeft: {Vector2.Distance(TouchOne, JoystickLeftPos) < JoystickL.sizeDelta.x * .75f}", 17);
            db.UpdateDebug($"SizeR: {JoystickR.sizeDelta.x} / {JoystickR.position}", 16);
            db.UpdateDebug($"SizeL: {JoystickL.sizeDelta.x} / {JoystickL.position}", 15);
            db.UpdateDebug($"DistR: {Vector2.Distance(TouchOne, JoystickRightPos)}", 14);
            db.UpdateDebug($"DistL: {Vector2.Distance(TouchOne, JoystickLeftPos)}", 13);
            db.UpdateDebug($"Control Mode: {controlsMode}", 12);
            db.UpdateDebug($"Ship Fire: {playerControl.InGame.ShipFire.triggered}", 11);
            db.UpdateDebug($"Touch Count: {Touch.activeTouches.Count}", 10);
            db.UpdateDebug($"Pos2: {TouchTwo}", 9);
            db.UpdateDebug($"Pos1: {TouchOne}", 8);
            db.UpdateDebug($"Rad: {TouchRad}", 7);
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
            TouchFireShip();
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
            if (Mathf.Approximately(Time.timeScale, 0f)) return;

            Vector2 touch = Camera.main.ViewportToWorldPoint(Camera.main.ScreenToViewportPoint(TouchOne));

            float distShip = Vector2.Distance(touch, mothership.transform.position);
            float distTarget = Vector2.Distance(touch, shootingTarget.transform.position);

            if (distShip * 1.5f > distTarget)
                closeFinger = 2;
            else
                closeFinger = 1;
        }

        public void TouchOnePos()
        {
            if (Mathf.Approximately(Time.timeScale, 0f) || closeFinger == 0 || TouchOne == Vector2.zero) return;

            if (closeFinger == 1)
                mothership.Move(TouchOne, TouchOneRadius());
            else
                shootingTarget.ControlTarget(TouchOne, TouchOneRadius());
        }

        public void TouchTwoPos()
        {
            if (Mathf.Approximately(Time.timeScale, 0f) || closeFinger == 0 || TouchTwo == Vector2.zero) return;

            if (closeFinger == 2)
                mothership.Move(TouchTwo, TouchOneRadius());
            else
                shootingTarget.ControlTarget(TouchTwo, TouchOneRadius());
        }

        private Vector2 TouchOneRadius()
        {
            if (Settings.GetObject().touchRay)
                return playerControl.Player.Move.ReadValue<Vector2>();
            else
                return Vector2.one * .04f;
        }

        private Vector2 TouchTwoRadius()
        {
            if (Settings.GetObject().touchRay)
                return playerControl.Player.Look.ReadValue<Vector2>();
            else
                return Vector2.one * .04f;
        }

        public void TouchFireShip()
        {
            if (!HUD.activeSelf) return;

            if (closeFinger == 1)
                playerControl.InGame.ShipFireTwo.performed += FireShip;
            else
                playerControl.InGame.ShipFire.performed += FireShip;
        }

        #endregion

        #region Touch & Joystick Input

        private void TouchJoystickInput()
        {
            if (controlsMode == 2)
            {
                JoystickTarget();
                if ((Vector2.Distance(TouchOne, JoystickRightPos) < JoystickR.sizeDelta.x * 2 && Vector2.Distance(TouchTwo, JoystickRightPos) > JoystickR.sizeDelta.x * 2) || JoystickRight != Vector2.zero)
                {
                    closeFinger = 2;
                    TouchTwoPos();
                }
                else
                {
                    closeFinger = 1;
                    TouchOnePos();
                }
            }
            else
            {
                JoystickMove();
                if ((Vector2.Distance(TouchOne, JoystickLeftPos) < JoystickL.sizeDelta.x * 2 && Vector2.Distance(TouchTwo, JoystickLeftPos) > JoystickL.sizeDelta.x * 2) || JoystickLeft != Vector2.zero)
                {
                    closeFinger = 1;
                    TouchTwoPos();
                }
                else
                {
                    closeFinger = 2;
                    TouchOnePos();
                }
                playerControl.InGame.ShipFire.performed += FireShip;
            }

        }

        #endregion

        #region Joystick Input

        private void JoystickInput()
        {
            JoystickMove();
            JoystickTarget();
            JoystickFireShip();
        }

        private void JoystickMove()
        {
            mothership.JoystickMove(JoystickLeft);
        }

        private void JoystickTarget()
        {
            shootingTarget.JoystickControlTarget(JoystickRight);
        }

        private void JoystickFireShip()
        {
            if (!HUD.activeSelf) return;
            playerControl.InGame.ShipFire.performed += FireShip;
        }

        #endregion

        #region Events Controls

        public void OpenDraw()
        {
            if (!HUD.activeSelf || _colorsInventory.drawColor == Color.black) return;
            HUD.SetActive(false);
            ColorHUD.SetActive(true);
            _lineDrawer.gameObject.SetActive(true);
            _lineDrawer.BulletTime(.5f);
        }

        public void OpenInventory()
        {
            if (!HUD.activeSelf) return;
            HUD.SetActive(false);
            InventoryHUD.SetActive(true);
            Time.timeScale = 0.5f;
        }

        public void CloseInventory()
        {
            Time.timeScale = 1;
            InventoryHUD.SetActive(false);
            HUD.SetActive(true);
        }

        public void DrawInput(InputAction.CallbackContext context)
        {
            _lineDrawer.DrawCallBack(context);
        }

        public void FireShip(InputAction.CallbackContext context)
        {
            _cyanPathDrawer.CyanGO();
        }

        public void FireShipButton()
        {
            if (!HUD.activeSelf) return;
            _cyanPathDrawer.CyanGO();
        }

        #endregion

        private Vector2 TouchOne => playerControl.InGame.TouchOne.ReadValue<Vector2>();
        private Vector2 TouchTwo => playerControl.InGame.TouchTwo.ReadValue<Vector2>();
        private Vector2 TouchRad => playerControl.InGame.TouchOneRadius.ReadValue<Vector2>();
        private Vector2 JoystickLeft => playerControl.Player.Move.ReadValue<Vector2>();
        private Vector2 JoystickRight => playerControl.Player.Look.ReadValue<Vector2>();
        private Vector2 JoystickRightPos => (Vector2)JoystickR.position - (Vector2.one * JoystickR.sizeDelta / 2);
        private Vector2 JoystickLeftPos => (Vector2)JoystickL.position + (Vector2.one * JoystickL.sizeDelta / 2);
    }
}