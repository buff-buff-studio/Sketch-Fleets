using UnityEngine;

namespace ManyTools.Variables
{
    [CreateAssetMenu(fileName = CreateMenus.StringVariableFileName, menuName = CreateMenus.StringVariableMenu, 
        order = CreateMenus.StringVariableOrder)]
    public class StringVariable : Variable<string>
    {
    }
}