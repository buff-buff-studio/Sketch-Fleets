using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    #region Var
    float velocity = 8;
    #endregion

    #region Unity Call Back
    void Start()
    {
        
    }

    /// <summary>
    /// <para>Faz a bala ir para frente e ser destruida se sair do mapa</para>
    /// </summary>
    void Update()
    {
        transform.position += (Vector3.right * velocity) * Time.deltaTime;
        if (transform.position.x > 9)
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
