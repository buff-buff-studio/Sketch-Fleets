using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectileScript : MonoBehaviour
{

    #region Unity Callbacks
    void Update()
    {
        Destroy(gameObject, 10);
        // Destroys projectile after 10 seconds of spawning
    }

    #endregion

    #region Collision

    private void OnCollisionEnter()
    {
        Destroy(gameObject);
        // Destroys projectile when it touches another Rigidbody
    }

    #endregion
}
