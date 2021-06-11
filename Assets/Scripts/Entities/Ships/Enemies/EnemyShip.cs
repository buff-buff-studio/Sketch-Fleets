using SketchFleets.Data;
using ManyTools.Variables;
using UnityEngine;
using UnityEngine.Serialization;

namespace SketchFleets.Enemies
{
    /// <summary>
    /// A class that controls an enemy ship
    /// </summary>
    public class EnemyShip : Ship<ShipAttributes>
    {
        #region Private Fields
        
        [FormerlySerializedAs("killsCount")] [SerializeField]
        private IntReference shipDeathCount;
        
        #endregion

        public override void Die()
        {
            base.Die();

            shipDeathCount.Value++;
        }
    }
}