using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManyTools.Variables;

public class ColorShips : MonoBehaviour
{
    /// <summary>
    /// WIP CODE
    /// </summary>

    #region Private Field
    [SerializeField]
    private FloatReference lifeShip;
    #endregion
    
    #region Public Field
    public float Life;
    public float LifeMax;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        Life = lifeShip;
        LifeMax = lifeShip;
    }
    #endregion

    #region Collider
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("EndMap"))
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
