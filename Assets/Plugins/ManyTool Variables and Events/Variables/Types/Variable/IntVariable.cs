using UnityEngine;

namespace ManyTools.Variables
{
    [CreateAssetMenu(fileName = CreateMenus.intVariableFileName, menuName = CreateMenus.intVariableMenu, 
        order = CreateMenus.intVariableOrder)]
    public class IntVariable : Variable<int>
    {
    }
}