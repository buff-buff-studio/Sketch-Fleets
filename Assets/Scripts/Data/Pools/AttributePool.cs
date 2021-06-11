using ManyTools.UnityExtended.WeightedPool;
using UnityEngine;

namespace SketchFleets.Data
{
    [CreateAssetMenu(fileName = CreateMenus.attributePoolFileName, menuName = CreateMenus.attributePoolMenuName,
        order = CreateMenus.attributePoolOrder)]
    public class AttributePool : WeightedPool<Attributes>
    {
        
    }
}