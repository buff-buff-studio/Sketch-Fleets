using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEvents : MonoBehaviour
{
    #region Public Fields
    public float velocity;
    public int shipSelect;
    public GameObject Ships;
    #endregion

    #region Unity Call Back
    /// <summary>
    /// <para>Seleção da nave</para>
    /// </summary>
    private void Update()
    {
        //Troca da nave selecionada
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
    #endregion
}
