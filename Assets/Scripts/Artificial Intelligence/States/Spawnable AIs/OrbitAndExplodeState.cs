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
        
        [HideInInspector]
        public bool explode;

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
            }
            else
            {
                Vector3 movement = Vector3.up * 
                                   (AI.Ship.Attributes.Speed * Time.deltaTime * Time.timeScale * launchSpeedMultiplier);
                transform.Translate(movement);
            }
        }

        #endregion

        public void LookAtTarget(Vector2 pos) //TODO: Remove
        {
            AI.Ship.Look(pos);
            base.StateUpdate();
            explode = true;
        }
        
        #region Unity Callbacks

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!explode) return;
            
            if (other.gameObject.CompareTag("Player") ||
                other.gameObject.CompareTag("PlayerSpawn") ||
                other.gameObject.CompareTag("bullet")) return;

            AI.Ship.Die();
        }

        private void OnDisable()
        {
            explode = false;
        }

        #endregion
    }
}