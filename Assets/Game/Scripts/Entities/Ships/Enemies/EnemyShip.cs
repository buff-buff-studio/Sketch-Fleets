using System.Collections;
using ManyTools.UnityExtended;
using ManyTools.Variables;
using SketchFleets.Data;
using SketchFleets.General;
using UnityEngine;

namespace SketchFleets.Enemies
{
    /// <summary>
    /// A class that controls an enemy ship
    /// </summary>
    public sealed class EnemyShip : Ship<ShipAttributes>
    {
        [SerializeField]
        private ColorReference enemyDeathColor = new ColorReference(Color.white);
        [SerializeField]
        private GameObjectReference enemyDeathBullet;
        
        /// <summary>
        /// Emerges the Poolable object from the pool
        /// </summary>
        /// <param name="position">The position at which to emerge the object</param>
        /// <param name="rotation">The rotation to emerge the object with</param>
        public override void Emerge(Vector3 position, Quaternion rotation)
        {
            base.Emerge(position, rotation);
            
            // TODO: This is a band-aid to fix a bug. Make sure to remove it later
            DelayProvider.Instance.DoDelayed(Die, 60f, GetInstanceID());
        }

        public override void Die()
        {
            base.Die();

            enemyDeathColor.Value = Attributes.ShipColor;
            enemyDeathBullet.Value = Attributes.DropedFire;
            LevelManager.Instance.CountShipDeath();
        }
        
        public void Lock(int lockMax, float lockTime)
        {
            lockHit++;
            if (lockHit >= lockMax)
            {
                lockHit = 0;
                StartCoroutine(LockState(lockTime));
            }
        }
        
        protected override IEnumerator LockState(float lockTime)
        {
            lockParent ??= GameObject.Find("LockShip").transform;
            transform.parent = lockParent;
            isLocked = true;
            
            do
            {
                yield return new WaitForSeconds(lockTime);
            } 
            while (lockHit != 0);
            
            isLocked = false;
            transform.parent = null;
        }
    }
}