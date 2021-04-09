using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    #region Public Fields

    public float moveSpeed = 10;

    public Rigidbody projectile;

    public float shootingCooldown = 1.5f;

    #endregion

    #region Unity Callbacks
    void Update()
    {
        Movement();
        Shooting();
    }
    private void OnCollisionEnter()
    {
        Destroy(gameObject);
        // Destroys enemy when a collision with another Rigidbody is detected
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// <para>M�todo para a movimenta��o do inimigo</para>
    /// Utiliza o public float moveSpeed para controlar a velocidade de movimento
    /// </summary>
    private void Movement()
    {
        transform.Translate(-Vector3.right * moveSpeed * Time.deltaTime);
        // Makes the enemy move only on the X axis. Right now it moves from right to left only.
    }

    /// <summary>
    /// <para>M�todo para o tiro/ataque inimigo</para>
    /// Utiliza o public float shootingCooldown para determinar o tempo entre tiros
    /// Utiliza o public Rigidbody projectile para determinar qual objeto ser� usado como proj�til
    /// </summary>
    private void Shooting()
    {
        shootingCooldown -= Time.deltaTime;

        if (shootingCooldown <= 0)
        {
            Rigidbody proj = Instantiate(projectile, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), Quaternion.identity);
            proj.velocity = -transform.right * moveSpeed * 1.2f;
            // The last value is there so the speed of the projectile will always be higher than the enemy itself.

            shootingCooldown = 1.5f;
            // Resets the timer for the next shot
        }
    }

    #endregion
}