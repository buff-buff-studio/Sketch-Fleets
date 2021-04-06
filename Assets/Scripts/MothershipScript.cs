using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothershipScript : MonoBehaviour
{

    public GlobalVar gv;
    public GameObject shipPrefab;
    int ships = 1;
    SpriteRenderer sprite;
    bool select;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gv.shipSelect == 0)
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
        }
        else
        {
            select = false;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject ship = (GameObject)Instantiate(shipPrefab, transform.position, transform.rotation);
            ships++;
            ship.GetComponent<ShipScript>().ShipNum = ships;
            ship.GetComponent<ShipScript>().gv = gv;
            ship.transform.parent = transform.parent;
        }

        if (select && sprite.color == Color.blue)
        {
            sprite.color = Color.green;
        }
        else if (!select)
        {
            sprite.color = Color.blue;
        }
    }
}
