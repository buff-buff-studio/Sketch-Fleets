using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limeAI : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private float limeH;
    [SerializeField]
    private float limeSpeed;
    private float limeY;
    private bool limeUp;
    [SerializeField]
    private Transform[] bulletSpawn;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject healingArea;
    private bool inCam = false;
    #endregion

    #region Public Fields
    public float LimeLife;
    public GameObject ShellPrefab;
    public Color ShellColor;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        StartCoroutine(ShootReload());
    }

    void Update()
    {
        LimeAI();
    }
    #endregion

    #region AI

    private void LimeAI()
    {
        limeY = transform.position.y;
        if (limeUp)
        {
            if(limeY < limeH)
            {
                Vector3 move = Vector3.up * (limeSpeed * Time.deltaTime);
                transform.Translate(move, Space.World);
            }
            else
            {
                limeUp = false;
            }
        }
        else
        {
            if (limeY > -limeH)
            {
                Vector3 move = Vector3.up * (limeSpeed * Time.deltaTime);
                transform.Translate(-move, Space.World);
            }
            else
            {
                limeUp = true;
            }
        }
        transform.eulerAngles += (Vector3.forward * (limeSpeed * Time.deltaTime))*10;

        if (LimeLife <= 0)
        {
            GameObject healing = (GameObject)Instantiate(healingArea, transform.position, transform.rotation);

            GameObject pencilShell = (GameObject)Instantiate(ShellPrefab, transform.position, transform.rotation);
            pencilShell.GetComponent<SpriteRenderer>().color = ShellColor;

            Destroy(gameObject);
        }
    }

    private void LimeShoot()
    {
        if (inCam)
        {
            GameObject bullet1 = (GameObject)Instantiate(bulletPrefab, bulletSpawn[0].position, bulletSpawn[0].rotation);
            GameObject bullet2 = (GameObject)Instantiate(bulletPrefab, bulletSpawn[1].position, bulletSpawn[1].rotation);
            GameObject bullet3 = (GameObject)Instantiate(bulletPrefab, bulletSpawn[2].position, bulletSpawn[2].rotation);
            GameObject bullet4 = (GameObject)Instantiate(bulletPrefab, bulletSpawn[3].position, bulletSpawn[3].rotation);
            bullet1.GetComponent<Rigidbody2D>().AddForce(-transform.up * 40, ForceMode2D.Impulse);
            bullet2.GetComponent<Rigidbody2D>().AddForce(transform.up * 40, ForceMode2D.Impulse);
            bullet3.GetComponent<Rigidbody2D>().AddForce(-transform.right * 40, ForceMode2D.Impulse);
            bullet4.GetComponent<Rigidbody2D>().AddForce(transform.right * 40, ForceMode2D.Impulse);
        }

        StartCoroutine(ShootReload());
    }

    IEnumerator ShootReload()
    {
        yield return new WaitForSeconds(1);
        LimeShoot();
    }
    #endregion

    #region Collider
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("bullet"))
        {
            LimeLife -= col.GetComponent<BulletController>().Attributes.DirectDamage;
        }
        else if (col.gameObject.CompareTag("EndMap"))
        {
            inCam = true;
        }
        else if (col.gameObject.CompareTag("CyanShip"))
        {
            Debug.Log(LimeLife);
            if (!col.GetComponent<cyanAI>().shoot)
            {
                LimeLife -= col.GetComponent<cyanAI>().Damage;
            }
            Debug.Log(col.GetComponent<cyanAI>().shoot);
            Debug.Log(LimeLife);
        }
    }
    #endregion
}
