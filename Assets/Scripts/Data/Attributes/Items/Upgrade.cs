using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that contains the attributes relating to an item
    /// </summary>
    [CreateAssetMenu(order = CreateMenus.itemAttributesOrder, fileName = CreateMenus.itemAttributesFileName, 
        menuName = CreateMenus.itemAttributesMenuName)]
    public class Upgrade : ScriptableObject
    {
        #region Private Fields
        [SerializeField]
        private string unlocalized_name;
        #endregion
    }
}