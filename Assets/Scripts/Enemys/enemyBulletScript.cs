using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBulletScript : MonoBehaviour
{
    public float Damage;

    #region Unity Callbacks
    private void Start()
    {
        Destroy(gameObject, 5);
    }
    #endregion

    #region Collider
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("EndMap"))
        {
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject, 10);
            transform.localScale = transform.localScale * .75f;
        }
    }
    #endregion
}
