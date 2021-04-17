using UnityEngine;

namespace ManyTools.Variables
{
    [CreateAssetMenu(fileName = CreateMenus.Vector2VariableFileName, menuName = CreateMenus.Vector2VariableMenu, 
        order = CreateMenus.Vector2VariableOrder)]
    public class Vector2Variable : Variable<Vector2>
    {
    }
}