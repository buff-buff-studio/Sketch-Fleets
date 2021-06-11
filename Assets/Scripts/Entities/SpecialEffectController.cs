using ManyTools.UnityExtended.Poolable;
using ManyTools.Variables;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SketchFleets.Entities
{
    /// <summary>
    /// A class that controls special effects
    /// </summary>
    [RequireComponent(typeof(AudioSource), typeof(ParticleSystem))]
    public class SpecialEffectController : PoolMember
    {
        #region Private Fields

        [Header("Additional Parameters")]
        [SerializeField, Tooltip("Whether the object should be randomly rotated on being spawned")]
        private BoolReference rotateOnAppear = new BoolReference(true);
        [SerializeField, Tooltip("The variance in scale of the special effect being spawned")]
        private FloatReference scaleVariation = new FloatReference(0.25f);
        
        private ParticleSystem visualParticleSystem;
        private AudioSource audioSource;

        private float lifetime;
        private Vector3 originalScale;

        #endregion

        #region PoolMember Overrides

        /// <summary>
        /// Emerges the Poolable object from the pool
        /// </summary>
        /// <param name="position">The position at which to emerge the object</param>
        /// <param name="rotation">The rotation to emerge the object with</param>
        public override void Emerge(Vector3 position, Quaternion rotation)
        {
            base.Emerge(position, rotation);
            
            // Applies optional effects
            if (rotateOnAppear)
            {
                RotateByRandomAmount();
            }

            if (!Mathf.Approximately(0f, scaleVariation))
            {
                VariateScale();
            }
            
            // Plays effects
            visualParticleSystem.Play();
            audioSource.Play();
            
            SubmergeDelayed(lifetime);
        }

        /// <summary>
        /// Submerges the Poolable object into the pool.
        /// </summary>
        public override void Submerge()
        {
            // Stops all effects
            visualParticleSystem.Clear();
            audioSource.Stop();
            
            base.Submerge();
        }

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            visualParticleSystem = GetComponent<ParticleSystem>();
            audioSource = GetComponent<AudioSource>();
            
            lifetime = CalculateLifetime();
            originalScale = transform.localScale;
        }

        #endregion
        
        #region Private Methods

        /// <summary>
        /// Returns the lifetime of this special effect object
        /// </summary>
        /// <returns>The longest duration out of the particle system and the audio clip lenght</returns>
        private float CalculateLifetime()
        {
            // Caches to avoid repeated marshalling
            ParticleSystem.MainModule main = visualParticleSystem.main;
            AudioClip clip = audioSource.clip;

            return clip == null ? main.duration : math.max(main.duration, clip.length);
        }

        /// <summary>
        /// Rotates the object randomly upon spawning
        /// </summary>
        private void RotateByRandomAmount()
        {
            transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
        }

        /// <summary>
        /// Variates the scale of the object
        /// </summary>
        private void VariateScale()
        {
            float scaleDeviation = Random.Range(scaleVariation * -1f, scaleVariation);
            Vector3 variantScale = new Vector3(originalScale.x + scaleDeviation, originalScale.y + scaleDeviation);
            transform.localScale = variantScale;
        }

        #endregion
    }
}