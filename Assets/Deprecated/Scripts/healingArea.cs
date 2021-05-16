using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManyTools.Variables;
using ManyTools.Events;

public class healingArea : MonoBehaviour
{
    [SerializeField]
    private FloatReference mothershipLife;
    [SerializeField]
    private GameEvent events;
    void Start()
    {
        Destroy(gameObject, 2);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "LimeEnemy(Clone)")
        {
            col.GetComponent<limeAI>().LimeLife += 25;
        }
        else if (col.gameObject.name == "PurpleEnemy(Clone)")
        {
            col.GetComponent<purpleAI>().PurpleLife += 5;
        }
        else if (col.gameObject.name == "OrangeEnemy(Clone)")
        {
            col.GetComponent<orangeAI>().OrangeLife += 10;
        }
        else if (col.gameObject.name == "MotherShip")
        {
            if(mothershipLife >= 985)
            {
                mothershipLife.Value += (1000 - mothershipLife);
            }
            else
            {
                mothershipLife.Value += 15;
            }

        }
    }

}
