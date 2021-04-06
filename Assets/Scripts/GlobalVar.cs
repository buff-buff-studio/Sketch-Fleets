using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVar : MonoBehaviour
{
    public float velocity;
    public int shipSelect;
    public GameObject Ships;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (shipSelect == 0 || shipSelect < Ships.transform.childCount - 1)
            {
                shipSelect++;
            }
            else
            {
                shipSelect = 0;
            }
        }
    }
}
