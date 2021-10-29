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
        public GameObject bulletPrefab;

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
            GenerateBullet();
            // The invoke here is beyond horrible in terms of performance, but I'd rather not spend
            // more time in this script, the deadline is looming
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

        #endregion

        #region Private Methods

        private void GenerateBullet()
        {
            GameObject cacheBullet = Attributes.Fire.Prefab;
            bulletPrefab = Instantiate(bulletPrefab, transform);
            bulletPrefab.GetComponent<SpriteRenderer>().sprite =
                cacheBullet.GetComponent<SpriteRenderer>().sprite;
            bulletPrefab.transform.localScale = cacheBullet.transform.localScale;
            bulletPrefab.gameObject.SetActive(false);
        }
        
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
            if (fireTimer > 0f || isLocked) return;

            for (int index = 0, upper = bulletSpawnPoints.Length; index < upper; index++)
            {
                PoolMember bullet = PoolManager.Instance.Request(bulletPrefab);
                bullet.Emerge(bulletSpawnPoints[index].position, bulletSpawnPoints[index].rotation);
                bullet.GetComponent<BulletController>().PlayerBuletVelocity = Attributes.Fire.Speed;

                bullet.GetComponent<SpriteRenderer>().color = spriteRenderer.material.GetColor(redMultiplier);
                bullet.transform.Rotate(0f, 0f,
                    Random.Range(Attributes.Fire.AngleJitter * -1f, Attributes.Fire.AngleJitter));
                bullet.gameObject.SetActive(true);
            }

            fireTimer = Attributes.Fire.Cooldown;
        }

        #endregion
    }
}