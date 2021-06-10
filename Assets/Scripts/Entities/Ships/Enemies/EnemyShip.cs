using SketchFleets.Data;
using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Enemies
{
    /// <summary>
    /// A class that controls an enemy ship
    /// </summary>
    public class EnemyShip : Ship<ShipAttributes>
    {
        #region Private Fields
        [SerializeField]
        private IntReference killsCount;
        #endregion

        public override void Die()
        {
            base.Die();

            killsCount.Value++;
        }
    }
}