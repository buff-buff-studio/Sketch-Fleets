using ManyTools.Variables;
using SketchFleets.AI;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// An AI state that orbits around the player and fires when he does so
    /// </summary>
    public class OrbitAndExplodeState : BaseOrbitState
    {
        #region Private Fields

        [SerializeField]
        private FloatReference launchSpeedMultiplier = new FloatReference(5f);
        
        private bool explode = false;

        #endregion

        #region State Implementation

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            if (!explode)
            {
                base.StateUpdate();

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    explode = true;
                }
            }
            else
            {
                Vector3 movement = Vector3.up * 
                                   (AI.Ship.Attributes.Speed * Time.deltaTime * Time.timeScale * launchSpeedMultiplier);
                transform.Translate(movement);
            }
        }

        #endregion

        #region Unity Callbacks

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!explode) return;
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerSpawn") ||
                other.gameObject.CompareTag("bullet")) return;
            
            AI.Ship.Die();
        }

        #endregion
    }
}