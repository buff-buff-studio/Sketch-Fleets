using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManyTools.Variables;

public class pencilShell : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private IntReference pencilNum;
    [SerializeField]
    private ColorReference shellColor;
    #endregion

    #region Collider
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("bullet") || col.gameObject.CompareTag("Player"))
        {
            pencilNum.Value++;
            shellColor.Value = GetComponent<SpriteRenderer>().color;
            Destroy(gameObject);
        }
    }
    #endregion
}
