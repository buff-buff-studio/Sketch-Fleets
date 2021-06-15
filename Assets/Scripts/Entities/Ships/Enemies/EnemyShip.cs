using SketchFleets.Data;
using ManyTools.Variables;
using SketchFleets.General;
using UnityEngine;
using UnityEngine.Serialization;

namespace SketchFleets.Enemies
{
    /// <summary>
    /// A class that controls an enemy ship
    /// </summary>
    public class EnemyShip : Ship<ShipAttributes>
    {
        public override void Die()
        {
            base.Die();

            LevelManager.Instance.CountShipDeath();
        }
    }
}