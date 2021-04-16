using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pencilShell : MonoBehaviour
{
    #region Collider
    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }
    #endregion
}
