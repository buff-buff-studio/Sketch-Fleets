using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script de movimento da nave
/// </summary>
public class ShipScript : MonoBehaviour
{
    #region Public Fields
    public ShipEvents gv;
    public GameObject bulletPrefab;
    public int ShipNum;
    #endregion

    #region Var
    SpriteRenderer sprite;
    bool select;
    bool bulletPos = false;
    #endregion

    #region Unity Call Back
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// <para>Movimento da nava e tiro da nave</para>
    /// </summary>
    void Update()
    {
        //Movimento da nave
        if (gv.shipSelect == ShipNum)
        {
            select = true;
            if (Input.GetKey(KeyCode.W) && transform.position.y < 3.8f)
            {
                transform.position += (Vector3.up * gv.velocity) * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.S) && transform.position.y > -3.8f)
            {
                transform.position += (Vector3.down * gv.velocity) * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.A) && transform.position.x > -8.25f)
            {
                transform.position += (Vector3.left * gv.velocity) * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D) && transform.position.y < 8.25f)
            {
                transform.position += (Vector3.right * gv.velocity) * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (bulletPos)
                {
                    GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.GetChild(0).position, transform.rotation);
                    bulletPos = false;
                }
                else
                {
                    GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.GetChild(1).position, transform.rotation);
                    bulletPos = true;
                }
            }
        }
        else
        {
            select = false;
        }

        //Demonstração que a nave esta selecionada
        if (select && sprite.color == Color.red)
        {
            sprite.color = Color.green;
        }
        else if (!select)
        {
            sprite.color = Color.red;
        }
    }
    #endregion
}
