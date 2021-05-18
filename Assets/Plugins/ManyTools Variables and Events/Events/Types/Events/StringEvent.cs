using UnityEngine;

namespace ManyTools.Events
{
    [CreateAssetMenu(fileName = CreateMenus.StringEventFilename, menuName = CreateMenus.StringEventMenu,
        order = CreateMenus.StringEventOrder)]
    public class StringEvent : GameEvent<string>
    {
        
    }
}