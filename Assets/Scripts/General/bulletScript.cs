using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManyTools.Variables;

public class bulletScript : MonoBehaviour
{
    public float Damage;
    #region Collider
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("EndMap"))
        {
            Destroy(gameObject);
        }else if (col.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject, 10);
            transform.localScale = transform.localScale * .75f;
        }
    }
    #endregion
}
