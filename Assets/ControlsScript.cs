using System.Collections;
using System.Collections.Generic;
using SketchFleets.Data;
using SketchFleets.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SketchFleets
{
    public class ControlsScript : MonoBehaviour
    {
        public Mothership _Mothership;
        public PauseScript _PauseScript;
        public CyanShoot _CyanShoot;
        public SpawnableShipAttributes MagentaShip;

        public List<OrbitAndAssistState> MagentaShips;

        public void Shoot(InputAction.CallbackContext context)
        {
            if (Time.timeScale == 1)
            {
                if (context.started)
                {
                    StartCoroutine(_Mothership.MobileFire());
                    if (_Mothership.SpawnMetaDatas.ContainsKey(MagentaShip))
                    {
                        foreach (var ship in _Mothership.SpawnMetaDatas[MagentaShip].CurrentlyActive)
                        {
                            Debug.Log(ship.name);
                            StartCoroutine(ship.GetComponent<OrbitAndAssistState>().MobileFire());
                        }
                    }
                }
                else if (context.canceled)
                {
                    StopCoroutine(_Mothership.MobileFire());
                    if (_Mothership.SpawnMetaDatas.ContainsKey(MagentaShip))
                    {
                        foreach (var ship in _Mothership.SpawnMetaDatas[MagentaShip].CurrentlyActive)
                        {
                            StopCoroutine(ship.GetComponent<OrbitAndAssistState>().MobileFire());
                        }
                    }
                }
            }
        }

        public void ShootCyan(InputAction.CallbackContext context)
        {
            if(_CyanShoot.GameHUD.activeSelf)
                _CyanShoot.CyanFire(context);
        }

        public void OpenDraw()
        {
            //if(_CyanShoot.GameHUD.activeSelf)
                //_Mothership.EnableOrDisableSpawnMenu(true);
        }

        public void Pause()
        {
            _PauseScript.PauseVoid(true);
        }
    }
}
