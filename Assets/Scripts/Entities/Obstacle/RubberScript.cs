using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManyTools.Variables;

public class RubberScript : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private FloatReference playerLife;
    private int damage;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        damage = GetComponent<ObstacleScript>().Damage;
    }

    void Update()
    {
        if(GetComponent<ObstacleScript>().Life <= 0)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Collider
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("bullet") || col.gameObject.CompareTag("EnemyBullet"))
        {
            GetComponent<ObstacleScript>().Life -= col.GetComponent<BulletController>().Attributes.DirectDamage;
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            playerLife.Value -= damage;
        }
        else if (col.gameObject.name == "LimeEnemy(Clone)")
        {
            col.GetComponent<limeAI>().LimeLife -= damage;
        }
        else if (col.gameObject.name == "PurpleEnemy(Clone)")
        {
            col.GetComponent<purpleAI>().PurpleLife -= damage;
        }
        else if (col.gameObject.name == "OrangeEnemy(Clone)")
        {
            col.GetComponent<orangeAI>().OrangeLife -= damage;
        }
        else if (col.name == "collider_back")
        {
            Destroy(gameObject, 5);
        }
    }
    #endregion
}
