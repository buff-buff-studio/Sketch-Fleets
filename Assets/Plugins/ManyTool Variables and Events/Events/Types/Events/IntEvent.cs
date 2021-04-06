using UnityEngine;

namespace ManyTools.Events
{
    [CreateAssetMenu(fileName = CreateMenus.IntEventFilename, menuName = CreateMenus.IntEventMenu,
        order = CreateMenus.IntEventOrder)]
    public class IntEvent : GameEvent<int>
    {
        
    }
}