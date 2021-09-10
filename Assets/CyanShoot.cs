using System.Collections;
using System.Collections.Generic;
using ManyTools.UnityExtended.Poolable;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SketchFleets
{
    public class CyanShoot : MonoBehaviour
    {
        private PlayerControl playerControl;
        private OrbitAndExplodeState CyanShip;
        private bool CloseUI;

        public Transform Mothership;
        public Transform InputTrail;
        public PoolManager _PoolManager;
        public GameObject CyanPrefab;
        public GameObject GameHUD;
        public GameObject CyanHUD;
        
        
        private void Awake()
        {
            playerControl = new PlayerControl();
            playerControl.Enable();
        }

        public void CyanFire(InputAction.CallbackContext context)
        {
            if (CyanShip == null)
            {
                try
                {
                    CyanShip = GameObject.Find("Cyan Ship(Clone)").GetComponent<OrbitAndExplodeState>();
                }catch
                {
                    Debug.Log("No Cyan");
                }
            } //TODO: Remove
            else
            {
                if (context.started)
                {
                    Vector2 pos = Camera.main.ScreenToWorldPoint(playerControl.Player.CyanLocate.ReadValue<Vector2>());
                    float dist = Vector2.Distance(pos, Mothership.position);
                    if (dist <= 2.8f)
                    {
                        GameHUD.SetActive(false);
                        CyanHUD.SetActive(true);
                        StartCoroutine(Trail());
                        StartCoroutine(TimerToClose());
                        Time.timeScale = .5f;
                    }
                }
                else if (context.canceled && CyanHUD.activeSelf)
                {
                    StopCoroutine(Trail());
                    Time.timeScale = 1;
                    GameHUD.SetActive(true);
                    CyanHUD.SetActive(false);
                    Vector2 pos = Camera.main.ScreenToWorldPoint(playerControl.Player.CyanLocate.ReadValue<Vector2>());
                    CyanShip.LookAtTarget(pos);
                    CyanShip = null;
                }

                if (CloseUI == true)
                    CloseUI = false;
            }
        }

        private IEnumerator Trail()
        {
            while (true)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(playerControl.Player.CyanLocate.ReadValue<Vector2>());
                InputTrail.position = new Vector3(pos.x, pos.y, -8);
                Debug.DrawLine(InputTrail.position, transform.position, Color.magenta);
                yield return null;
            }
        }

        private IEnumerator TimerToClose()
        {
            while (!CloseUI)
            {
                CloseUI = true;
                yield return new WaitForSeconds(1);
            }
            Time.timeScale = 1;
            GameHUD.SetActive(true);
            CyanHUD.SetActive(false);
        }
    }
}
