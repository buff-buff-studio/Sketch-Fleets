using UnityEngine;

namespace ManyTools.Variables
{
    [CreateAssetMenu(fileName = CreateMenus.boolVariableFileName, menuName = CreateMenus.boolVariableMenu, 
        order = CreateMenus.boolVariableOrder)]
    public class BoolVariable : Variable<bool>
    {
    }
}