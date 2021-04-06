using UnityEngine;

namespace ManyTools.Variables
{
    [CreateAssetMenu(fileName = CreateMenus.Vector3VariableFileName, menuName = CreateMenus.Vector3VariableMenu, 
        order = CreateMenus.Vector3VariableOrder)]
    public class Vector3Variable : Variable<Vector3>
    {
    }
}