using UnityEngine;

namespace ManyTools.Variables
{
    [CreateAssetMenu(fileName = CreateMenus.ColorVariableFileName, menuName = CreateMenus.ColorVariableMenu, 
        order = CreateMenus.ColorVariableOrder)]
    public class ColorVariable : Variable<Color>
    {
    }
}