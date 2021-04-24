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
    [SerializeField]
    private FloatReference explosionDamage;

    private int cyanShips;

    [SerializeField]
    private FloatReference timeReload;
    private bool shootTime = true;
    #endregion
    #region Public Fields
    public GameObject BulletPrefab;
    public List<GameObject> CyanShips;
    public Transform BulletSpawn;
    public Transform CyanShipsSpawner;
    public lifeBar lb;
    public GameObject DeathScene;
    #endregion

    #region Unity Callbacks
    void Update()
    {
        if(Input.GetAxis("CircleOpen") == 0)
        {
            MothershipShoot();
            MothershipCyanShoot();
        }
        if(life <= 0)
        {
            DeathScene.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            MothershipMovement();
        }
        //Debug.Log(life);
    }
    #endregion

    #region Commands Mothership
    /// <summary>
    /// WASD move the ship
    /// Ships look at mouse position
    /// </summary>
    private void MothershipMovement()
    {
        float TimeVelocity;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            TimeVelocity = (speed * 2) * Time.deltaTime;
        }
        else
        {
            TimeVelocity = speed * Time.deltaTime;
        }

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
        if (Input.GetAxis("Fire1") == 1)
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
        if (Input.GetAxis("Fire2") == 1)
        {          
            if (CyanShips.Count > 0)
            {
                Rigidbody2D _cyan = CyanShips[0].GetComponent<Rigidbody2D>();
                _cyan.AddForce(_cyan.transform.GetChild(0).up * bulletForce, ForceMode2D.Impulse);
                _cyan.transform.parent = transform.parent;
                _cyan.GetComponent<cyanAI>().shoot = false;
                GetComponent<ShipGenerator>().CyanShips--;
                CyanShips.Remove(CyanShips[0]);
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("EnemyBullet"))
        {
            life.Value -= col.GetComponent<enemyBulletScript>().Damage;
            lb.lifeBarUpdate();
        }
        else if (col.gameObject.CompareTag("Enemy"))
        {
            life.Value = life - (explosionDamage * col.transform.localScale.x);
            Destroy(col.gameObject);
            lb.lifeBarUpdate();
        }
    }
}
