using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    #region Movement Vars

    public float moveSpeed = 10;

    #endregion

    #region Shooting Vars

    public Rigidbody projectile;
    // The projectile that will be instantiated

    public float shootingCooldown = 1.5f;
    // Value equals the amount of time between each shot/attack

    #endregion

    #region Unity Callbacks
    void Update()
    {
        Movement();
        Shooting();

    }

    #endregion

    #region Private Method - Movement
    private void Movement()
    {
        transform.Translate(-Vector3.right * moveSpeed * Time.deltaTime);
        // Makes the enemy move only on the X axis. Right now it moves from right to left only.

    }

    #endregion

    #region Private Method - Shooting

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

    #region Collision
    private void OnCollisionEnter()
    {
        Destroy(gameObject);
        // Destroys enemy when a collision with another Rigidbody is detected
    }

    #endregion
}
