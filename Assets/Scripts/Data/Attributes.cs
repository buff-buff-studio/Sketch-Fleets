using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that holds attributes about entities and items
    /// </summary>
    public class Attributes : ScriptableObject
    {
        #region Private Fields

        [SerializeField]
        protected StringReference _name;
        [SerializeField]
        [Multiline]
        protected string _description;

        #endregion

        #region Properties

        public string Description
        {
            get => _description;
        }

        public StringReference Name
        {
            get => _name;
        }

        #endregion

    }
}