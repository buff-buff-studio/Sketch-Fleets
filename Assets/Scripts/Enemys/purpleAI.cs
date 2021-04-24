using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class purpleAI : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private Transform bulletSpawn;
    [SerializeField]
    private GameObject bulletPrefab;
    private bool inCam = false;
    #endregion

    #region Public Fields
    public float PurpleLife;
    public GameObject ShellPrefab;
    public Color ShellColor;
    #endregion
    void Start()
    {
        StartCoroutine(ShootReload());   
    }

    // Update is called once per frame
    void Update()
    {
        if(PurpleLife <= 0)
        {
            GameObject pencilShell = (GameObject)Instantiate(ShellPrefab, transform.position, transform.rotation);
            pencilShell.GetComponent<SpriteRenderer>().color = ShellColor;

            Destroy(gameObject);
        }
    }

    private void PurpleAI()
    {
        if (inCam)
        {
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(-transform.up * 40, ForceMode2D.Impulse);
        }

        StartCoroutine(ShootReload());
    }

    IEnumerator ShootReload()
    {
        yield return new WaitForSeconds(.3f);
        PurpleAI();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("bullet"))
        {
            PurpleLife -= col.GetComponent<playerBulletScript>().Damage;
            col.GetComponent<playerBulletScript>().Damage = 0;
        }
        else if(col.gameObject.CompareTag("EndMap"))
        {
            inCam = true;
        }
        else if (col.gameObject.CompareTag("CyanShip"))
        {
            if (!col.GetComponent<cyanAI>().shoot)
            {
                PurpleLife -= col.GetComponent<cyanAI>().Damage;
            }
        }
    }
}
