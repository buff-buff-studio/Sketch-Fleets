using ManyTools.UnityExtended.Poolable;
using UnityEngine;
using ManyTools.Variables;

namespace SketchFleets.Entities
{
    /// <summary>
    /// A class that controls the 
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class PencilShell : PoolMember, ICollectible
    {
        #region Private Fields

        [Header("Shell Parameters")]
        [SerializeField]
        private ColorReference shellColor = new ColorReference(Color.white);
        [SerializeField]
        private IntReference shellWorth = new IntReference(1);
        [SerializeField]
        private GameObject collectEffect;

        [Header("Visual Parameters")]
        [SerializeField]
        private FloatReference spinSpeed = new FloatReference(5f);

        [Header("Player Reference Variables")]
        [SerializeField]
        [Tooltip("The amount of player shells owned by the player")]
        private IntReference playerShells;
        [SerializeField]
        [Tooltip("The color of the last shell collected by the player")]
        private ColorReference playerShellColor;

        private Transform cachedTransform;
        private SpriteRenderer spriteRenderer;

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            cachedTransform = transform;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            cachedTransform.Rotate(0f, 0f, spinSpeed * Time.deltaTime * Time.timeScale);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player")) return;
            Collect();
        }

        #endregion

        #region ICollectible Implementation

        /// <summary>
        /// Applies all necessary effects upon collection
        /// </summary>
        public void Collect()
        {
            // Adds value and updates color on player HUD
            playerShells.Value += shellWorth.Value;
            playerShellColor.Value = shellColor.Value;

            // If there is a collect effect, spawn it
            if (collectEffect != null)
            {
                Instantiate(collectEffect, cachedTransform.position, cachedTransform.rotation);
            }

            // Sends its back to the pool
            Submerge();
        }

        #endregion

        #region PoolMember Overrides

        /// <summary>
        /// Emerges the Poolable object from the pool
        /// </summary>
        /// <param name="position">The position at which to emerge the object</param>
        /// <param name="rotation">The rotation to emerge the object with</param>
        public override void Emerge(Vector3 position, Quaternion rotation)
        {
            shellColor.Value = Color.white;
            shellWorth.Value = 1;

            base.Emerge(position, rotation);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the shell's color
        /// </summary>
        /// <param name="dropColor">The color to set to</param>
        public void SetDropColor(Color dropColor)
        {
            // if (spriteRenderer == null)
            // {
            //     spriteRenderer = GetComponent<SpriteRenderer>();
            // }
            
            spriteRenderer.color = dropColor;
        }

        #endregion
    }
}
