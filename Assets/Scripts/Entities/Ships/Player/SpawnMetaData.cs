using System.Collections;
using System.Collections.Generic;
using ManyTools.Variables;
using SketchFleets.Data;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// A class that contains data about spawned ships
    /// </summary>
    public class SpawnMetaData
    {
        #region Private Fields

        private SpawnableShipAttributes shipType;
        private List<Ship<SpawnableShipAttributes>> currentlyActive;
        private FloatReference summonTimer = new FloatReference(0f);

        #endregion

        #region Properties

        public FloatReference SummonTimer
        {
            get => summonTimer;
            set => summonTimer = value;
        }

        public SpawnableShipAttributes ShipType
        {
            get => shipType;
            set => shipType = value;
        }

        public List<Ship<SpawnableShipAttributes>> CurrentlyActive
        {
            get => currentlyActive;
            set => currentlyActive = value;
        }

        #endregion

        #region Constructor
        
        public SpawnMetaData(SpawnableShipAttributes shipType)
        {
            this.shipType = shipType;
        }

        #endregion
    }
}
