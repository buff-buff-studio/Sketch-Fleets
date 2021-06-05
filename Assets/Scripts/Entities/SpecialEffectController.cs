using ManyTools.UnityExtended.Poolable;
using Unity.Mathematics;
using UnityEngine;

namespace SketchFleets.Entities
{
    /// <summary>
    /// A class that controls special effects
    /// </summary>
    [RequireComponent(typeof(AudioSource), typeof(ParticleSystem))]
    public class SpecialEffectController : PoolMember
    {
        #region Private Fields

        private ParticleSystem particleSystem;
        private AudioSource audioSource;

        private float lifetime;

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
            
            // Plays effects
            particleSystem.Play();
            audioSource.Play();
            
            SubmergeDelayed(lifetime);
        }

        /// <summary>
        /// Submerges the Poolable object into the pool.
        /// </summary>
        public override void Submerge()
        {
            // Stops all effects
            particleSystem.Clear();
            audioSource.Stop();
            
            base.Submerge();
        }

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            lifetime = CalculateLifetime();
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
            ParticleSystem.MainModule main = particleSystem.main;
            AudioClip clip = audioSource.clip;

            return clip == null ? main.duration : math.max(main.duration, clip.length);
        }

        #endregion
    }
}
