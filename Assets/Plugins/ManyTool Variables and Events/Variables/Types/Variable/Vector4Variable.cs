using UnityEngine;

namespace ManyTools.Variables
{
    [CreateAssetMenu(fileName = CreateMenus.Vector4VariableFileName, menuName = CreateMenus.Vector4VariableMenu, 
        order = CreateMenus.Vector4VariableOrder)]
    public class Vector4Variable : Variable<Vector4>
    {
    }
}