using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManyTools.Variables;

public class MeteorScript : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private FloatReference playerLife;
    private int damage;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        float size = Random.Range(0, .75f);
        transform.localScale += new Vector3(size, size, size);
        GetComponent<Rigidbody2D>().AddForce(-transform.right * 55, ForceMode2D.Impulse);
        damage = GetComponent<ObstacleScript>().Damage;
        damage = (int)(damage + (float)damage * size);
        Debug.Log((damage));
    }
    #endregion

    #region Collider
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerLife.Value -= damage;
        }
        else if(col.gameObject.name == "LimeEnemy(Clone)")
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
        }else if (col.name == "collider_back")
        {
            Destroy(gameObject, 1);
        }
    }
    #endregion
}
