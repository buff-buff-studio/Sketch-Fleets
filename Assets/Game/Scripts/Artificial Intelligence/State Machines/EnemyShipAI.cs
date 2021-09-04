using ManyTools.UnityExtended;
using SketchFleets.Enemies;
using UnityEngine;

namespace SketchFleets.AI
{
    /// <summary>
    /// A state machine that controls the AI of a ship
    /// </summary>
    [RequireComponent(typeof(EnemyShip))]
    public class EnemyShipAI : StateMachine
    {
        #region Private Fields

        private GameObject player;
        private EnemyShip ship;

        #endregion

        #region Properties

        public GameObject Player => player;

        public EnemyShip Ship => ship;

        #endregion

        #region Unity Callbacks

        protected override void Start()
        {
            player = GameObject.FindWithTag("Player");
            ship = GetComponent<EnemyShip>();
            
            base.Start();
        }

        #endregion
    }
}