using ManyTools.Variables;
using SketchFleets.Data;
using UnityEngine;

namespace SketchFleets.Variables
{
    [CreateAssetMenu(fileName = CreateMenus.bulletAttributeVariableFileName, 
        menuName = CreateMenus.bulletAttributeVariableMenuName, order = CreateMenus.bulletAttributeVariableOrder)]
    public sealed class BulletAttributesVariable : Variable<BulletAttributes>
    {
    }
}