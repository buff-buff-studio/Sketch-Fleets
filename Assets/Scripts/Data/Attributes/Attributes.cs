using ManyTools.Variables;
using UnityEngine;
using UnityEngine.Serialization;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that holds attributes about entities and items
    /// </summary>
    public class Attributes : ScriptableObject
    {
        #region Private Fields

        [Header("Core Properties")]
        [SerializeField]
        protected StringReference objectName = new StringReference("Name");
        [SerializeField]
        [Multiline]
        protected string description;
        [SerializeField]
        protected GameObject prefab;

        #endregion

        #region Properties

        public GameObject Prefab => prefab;

        public string Description => description;

        public StringReference Name => objectName;

        #endregion
    }
}