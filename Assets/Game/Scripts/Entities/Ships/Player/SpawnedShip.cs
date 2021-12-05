using ManyTools.Events;
using ManyTools.UnityExtended;
using ManyTools.UnityExtended.Poolable;
using SketchFleets.Data;
using SketchFleets.General;
using UnityEngine;

namespace SketchFleets.Entities
{
    /// <summary>
    /// A class that controls a spawned ship
    /// </summary>
    public sealed class SpawnedShip : Ship<SpawnableShipAttributes>
    {
        #region Private Fields

        private BulletAttributes bulletOverride;

        #endregion

        #region Properties

        public int SpawnNumber { get; set; }

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
            Attributes.OnShipSpawned.Invoke();
            DelayProvider.Instance.DoDelayed(EmergeSpawnEffect, 0.1f, GetInstanceID());
        }

        #endregion

        #region Ship Overrides

        /// <summary>
        /// Makes the ship die
        /// </summary>
        public override void Die()
        {
            LevelManager.Instance.Player.RemoveActiveSummon(this);

            base.Die();
        }

        protected override void PlayFireSound()
        {
            if (bulletOverride.FireSound == null || !(Random.Range(0f, 1f) > bulletOverride.MuteChance)) return;
            soundSource.pitch = 1f + Random.Range(bulletOverride.PitchVariation * -1f, bulletOverride.PitchVariation);
            soundSource.PlayOneShot(bulletOverride.FireSound);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Overrides the ship's current bullet
        /// </summary>
        /// <param name="bullet">The bullet to override with</param>
        public void OverrideBullet(BulletAttributes bullet)
        {
            bulletOverride = bullet;
        }

        #endregion
        
        #region Private Methods

        /// <summary>
        /// Emerges a spawn effect at the ship's position
        /// </summary>
        private void EmergeSpawnEffect()
        {
            Transform cachedTransform = transform;
            PoolMember spawn = PoolManager.Instance.Request(Attributes.SpawnEffect);
            spawn.Emerge(cachedTransform.position, cachedTransform.rotation);

            ParticleSystem spawnCache = spawn.GetComponent<ParticleSystem>();
            spawnCache.startColor = shipColor;
        }

        public override void Fire()
        {
            if (fireTimer > 0f) return;

            for (int index = 0, upper = bulletSpawnPoints.Length; index < upper; index++)
            {
                PoolMember bullet = PoolManager.Instance.Request(bulletOverride.Prefab);
                bullet.Emerge(bulletSpawnPoints[index].position, bulletSpawnPoints[index].rotation);

                bullet.GetComponent<SpriteRenderer>().color = spriteRenderer.material.GetColor(redMultiplier);
                bullet.transform.Rotate(0f, 0f,
                    Random.Range(bulletOverride.AngleJitter * -1f, bulletOverride.AngleJitter));
                bullet.gameObject.SetActive(true);
            }

            PlayFireSound();

            fireTimer = bulletOverride.Cooldown;
        }

        #endregion
    }
}