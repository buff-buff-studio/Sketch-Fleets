using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManyTools.Variables;

public class ColorShips : MonoBehaviour
{
    #region Private Field
    [SerializeField]
    private FloatReference _lifeShip;
    #endregion
    #region Public Field
    public float _life;
    public float _lifeMax;
    #endregion
    #region Unity Callback
    void Start()
    {
        _life = _lifeShip;
        _lifeMax = _lifeShip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
    #region Collider
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "EndMap")
        {
            Destroy(gameObject);
        }
        else if (col.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
