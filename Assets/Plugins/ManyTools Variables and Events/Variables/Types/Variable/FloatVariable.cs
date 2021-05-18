using UnityEngine;

namespace ManyTools.Variables
{
    [CreateAssetMenu(fileName = CreateMenus.floatVariableFileName, menuName = CreateMenus.floatVariableMenu, 
        order = CreateMenus.floatVariableOrder)]
    public class FloatVariable : Variable<float>
    {
    }
}