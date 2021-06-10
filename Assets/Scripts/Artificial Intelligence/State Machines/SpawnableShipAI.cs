using ManyTools.UnityExtended;
using SketchFleets.Enemies;
using SketchFleets.Entities;
using UnityEngine;

namespace SketchFleets.AI
{
    /// <summary>
    /// A state machine that controls the AI of a ship
    /// </summary>
    [RequireComponent(typeof(SpawnedShip))]
    public class SpawnableShipAI : StateMachine
    {
        #region Private Fields

        private GameObject player;
        private SpawnedShip ship;
        private Camera mainCamera;

        #endregion

        #region Properties

        public GameObject Player => player;
        public SpawnedShip Ship => ship;
        public Camera MainCamera => mainCamera;

        #endregion
        
        #region Unity Callbacks

        protected override void Start()
        {
            mainCamera = Camera.main;
            player = GameObject.FindWithTag("Player");
            ship = GetComponent<SpawnedShip>();
            
            base.Start();
        }

        #endregion
    }
}