using SketchFleets.Data;

namespace SketchFleets.Entities
{
    /// <summary>
    /// A class that controls a spawned ship
    /// </summary>
    public class SpawnedShip : Ship<SpawnableShipAttributes>
    {
        #region Properties

        public int SpawnNumber { get; set; }

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
    }
}