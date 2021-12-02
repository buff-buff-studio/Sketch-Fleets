using ManyTools.Variables;
using SketchFleets.Data;
using SketchFleets.Systems;
using SketchFleets.Systems.DeathContext;
using SketchFleets.Variables;
using UnityEngine;

namespace SketchFleets.Enemies
{
    /// <summary>
    /// A class that controls an enemy ship
    /// </summary>
    public sealed class EnemyShip : Ship<ShipAttributes>
    {
        #region Private Fields

        [SerializeField]
        private ColorReference enemyDeathColor = new ColorReference(Color.white);

        [SerializeField]
        private BulletAttributesReference droppedBullet;


        #endregion

        #region Properties

        public ShipSpawner Spawner { get; set; }

        #endregion

        #region Public Methods

        public override void Die()
        {
            base.Die();
            Spawner.CountShipDeath(this);
            DropColor();
        }

        /// <summary>
        /// Drops the ship's color
        /// </summary>
        private void DropColor()
        {
            if (LatestDamageContext != DamageContext.PlayerBullet && LatestDamageContext != DamageContext.PlayerCollision) return;
            enemyDeathColor.Value = Attributes.ShipColor;

            droppedBullet.Value = Attributes.DroppedFire;
        }

        #endregion
    }
}