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

        #endregion

        #region Properties

        public bool Explode { get; private set; }

        #endregion

        #region State Implementation

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            if (!Explode)
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

        #region Unity Callbacks

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!Explode) return;
            
            if (other.gameObject.CompareTag("Player") ||
                other.gameObject.CompareTag("PlayerSpawn") ||
                other.gameObject.CompareTag("bullet"))
            {
                return;
            }

            Explode = false;
            AI.Ship.Die();
        }

        private void OnEnable()
        {
            Explode = false;
        }

        private void OnDisable()
        {
            Explode = false;
        }

        #endregion

        #region Public Methods

        public void LightFuse()
        {
            Explode = true;
        }
        
        public void LookAtTarget(Vector2 pos) //TODO: Remove
        {
            AI.Ship.Look(pos);
            Explode = true;
        }

        #endregion
    }
}