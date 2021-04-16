using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManyTools.Variables;

public class Mothership : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private FloatReference life;
    [SerializeField]
    private FloatReference speed;
    [SerializeField]
    private FloatReference bulletForce;

    private int cyanShips;

    [SerializeField]
    private FloatReference timeReload;
    private bool shootTime = true;
    #endregion
    #region Public Fields
    public GameObject BulletPrefab;
    public Transform BulletSpawn;
    public Transform CyanShipsSpawner;
    #endregion

    #region Unity Callbacks
    void Update()
    {
        MothershipMovement();
        MothershipShoot();
        MothershipCyanShoot();
    }
    #endregion

    #region Commands Mothership
    /// <summary>
    /// WASD move the ship
    /// Ships look at mouse position
    /// </summary>
    private void MothershipMovement()
    {
        float TimeVelocity = speed * Time.deltaTime;
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            transform.Translate(move * TimeVelocity, Space.World);

        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    /// <summary>
    /// Mouse 0 shoot with the Mothership
    /// </summary>
    private void MothershipShoot()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (shootTime)
            {
                GameObject bullet = (GameObject)Instantiate(BulletPrefab, BulletSpawn.position, transform.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(BulletSpawn.up * bulletForce, ForceMode2D.Impulse);

                shootTime = false;
                StartCoroutine(ShootTimer());
            }
        }
    }

    /// <summary>
    /// Mouse 1 launch the cyan ship
    /// </summary>
    private void MothershipCyanShoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            cyanShips = GetComponent<ShipGenerator>().CyanShips;            
            if (cyanShips > 0)
            {
                Rigidbody2D _cyan = CyanShipsSpawner.GetChild(2).GetComponent<Rigidbody2D>();
                _cyan.AddForce(_cyan.transform.GetChild(1).up * bulletForce, ForceMode2D.Impulse);
                _cyan.transform.parent = transform.parent;
                GetComponent<ShipGenerator>().CyanShips--;
            }

        }
    }
    #endregion

    #region Shoot Reload
    /// <summary>
    /// Timer
    /// </summary>
    IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(timeReload);
        shootTime = true;
    }
    #endregion
}
