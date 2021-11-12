using ManyTools.UnityExtended.WeightedPool;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A weighed pool of ship formations
    /// </summary>
    [CreateAssetMenu(order = CreateMenus.shipFormationPoolOrder, fileName = CreateMenus.shipFormationPoolFileName,
        menuName = CreateMenus.shipFormationPoolMenuName)]
    public sealed class FormationPool : WeightedPool<ShipFormation>
    {
        
    }
}