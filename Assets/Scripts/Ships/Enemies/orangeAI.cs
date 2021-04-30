using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orangeAI : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private float orangeSpeed;
    [SerializeField]
    private int orangeGen = 3;
    [SerializeField]
    private Transform[] orangeSpawn;
    [SerializeField]
    private GameObject orangePrefab;
    private bool inv;
    #endregion

    #region Public Fields
    public float OrangeLife;
    public Transform Mothership;
    public GameObject ShellPrefab;
    public Color ShellColor;
    public bool InCam;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        if(orangeGen == 3)
        {
            orangeGen = Random.Range(2, 4);
        }
        StartCoroutine(Invincible());
    }

    void Update()
    {
        OrangeAI();
    }
    #endregion

    #region AI
    /// <summary>
    /// Orange Ship look at Mothership
    /// Orange Ship go to Mothership
    /// Death and subdivide
    /// </summary>
    private void OrangeAI()
    {
        if (InCam)
        {
            var dir = Mothership.position - transform.position;
            var angle = Mathf.Atan2(dir.x, -dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.position = Vector2.MoveTowards(transform.position, Mothership.position, orangeSpeed * Time.deltaTime);
        }

        if(OrangeLife <= 0)
        {
            if (orangeGen > 1)
            {
                GameObject orange1 = (GameObject)Instantiate(orangePrefab, orangeSpawn[0].position, transform.rotation);
                GameObject orange2 = (GameObject)Instantiate(orangePrefab, orangeSpawn[1].position, transform.rotation);

                orange1.transform.localScale = transform.localScale * .6f;
                orange2.transform.localScale = transform.localScale * .6f;

                orange1.GetComponent<orangeAI>().orangeGen--;
                orange2.GetComponent<orangeAI>().orangeGen--;

                orange1.GetComponent<orangeAI>().OrangeLife = 75;
                orange2.GetComponent<orangeAI>().OrangeLife = 75;

                orange1.GetComponent<orangeAI>().Mothership = Mothership;
                orange2.GetComponent<orangeAI>().Mothership = Mothership;
            }
            else
            {
                GameObject pencilShell = (GameObject)Instantiate(ShellPrefab, transform.position, transform.rotation);
                pencilShell.GetComponent<SpriteRenderer>().color = ShellColor;
            }
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Receive Damage
    /// </summary>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("bullet") && !inv)
        {
            OrangeLife -= col.GetComponent<playerBulletScript>().Damage;
            col.GetComponent<playerBulletScript>().Damage = 0;
        }
        else if (col.gameObject.CompareTag("EndMap"))
        {
            InCam = true;
        }
        else if (col.gameObject.CompareTag("CyanShip"))
        {
            if (!col.GetComponent<cyanAI>().shoot)
            {
                OrangeLife -= col.GetComponent<cyanAI>().Damage;
            }
        }
    }

    /// <summary>
    /// Invincible mode when born 
    /// </summary>
    IEnumerator Invincible()
    {
        inv = true;
        yield return new WaitForSeconds(1.2f);
        inv = false;
    }
    #endregion
}
