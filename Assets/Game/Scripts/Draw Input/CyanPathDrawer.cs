using System;
using System.Collections;
using SketchFleets.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SketchFleets
{
    public sealed class CyanPathDrawer : MonoBehaviour
    {
        #region Private Fields

        [SerializeField]
        private Mothership mothership;

        [SerializeField]
        private Transform inputTrail;

        [SerializeField]
        private GameObject gameHUD;

        [SerializeField]
        private GameObject cyanHUD;
        
        [HideInInspector]
        public OrbitAndExplodeState CyanShip;
        private bool CloseUI;
        private Camera mainCameraCache;

        #endregion

        #region Public Fields

        public Button CyanButton;

        #endregion

        #region Properties

        public GameObject GameHUD => gameHUD;

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            mainCameraCache = Camera.main;
        }

        private void Update()
        {
            try
            {
                CyanShip = mothership.GetCyanShip();
                CyanButton.interactable = true;
            }
            catch
            {
                CyanButton.interactable = false;
            }
        }

        #endregion

        public void CyanGO()
        {
            try
            {
                CyanShip = mothership.GetCyanShip();
                CyanShip.LookAtTarget(mothership._ShootingTarget.targetPoint.position);
                CyanShip = null;
            }
            catch
            {
                CyanButton.interactable = false;
            }
        }

        /*
        public void CyanFire(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Vector2 pos = mainCameraCache.ScreenToWorldPoint(playerControl.Player.CyanLocate.ReadValue<Vector2>());
                float dist = Vector2.Distance(pos, mothership.transform.position);

                if (dist <= 2.8f)
                {
                    gameHUD.SetActive(false);
                    cyanHUD.SetActive(true);
                    StartCoroutine(Trail());
                    StartCoroutine(TimerToClose());
                    Time.timeScale = .5f;
                }
            }
            else if (context.canceled && cyanHUD.activeSelf)
            {
                StopCoroutine(Trail());
                Time.timeScale = 1;
                gameHUD.SetActive(true);
                cyanHUD.SetActive(false);
                Vector2 pos = mainCameraCache.ScreenToWorldPoint(playerControl.Player.CyanLocate.ReadValue<Vector2>());

                CyanShip = mothership.GetCyanShip();
                CyanShip.LookAtTarget(pos);
                CyanShip.LightFuse();
                
                CyanShip = null;
            }

            if (CloseUI)
            {
                CloseUI = false;
            }
        }

        private IEnumerator Trail()
        {
            while (true)
            {
                Vector2 pos = mainCameraCache.ScreenToWorldPoint(playerControl.Player.CyanLocate.ReadValue<Vector2>());
                inputTrail.position = new Vector3(pos.x, pos.y, -8);
                Debug.DrawLine(inputTrail.position, transform.position, Color.magenta);
                yield return null;
            }
        }

*/
        private IEnumerator TimerToClose()
        {
            WaitForSeconds wait = new WaitForSeconds(1f);
            
            while (!CloseUI)
            {
                CloseUI = true;
                yield return wait;
            }

            Time.timeScale = 1;
            gameHUD.SetActive(true);
            cyanHUD.SetActive(false);
        }
    }
}