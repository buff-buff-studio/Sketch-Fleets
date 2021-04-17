using UnityEngine;

namespace ManyTools.Events
{
    [CreateAssetMenu(fileName = CreateMenus.ColorVariableFileName, menuName = CreateMenus.ColorEventMenu,
        order = CreateMenus.ColorEventOrder)]
    public class ColorEvent : GameEvent<Color>
    {
        
    }
}