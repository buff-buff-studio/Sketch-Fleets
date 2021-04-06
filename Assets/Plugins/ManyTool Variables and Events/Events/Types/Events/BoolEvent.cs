using UnityEngine;

namespace ManyTools.Events
{
    [CreateAssetMenu(fileName = CreateMenus.BoolEventFilename, menuName = CreateMenus.BoolEventMenu,
        order = CreateMenus.BoolEventOrder)]
    public class BoolEvent : GameEvent<bool>
    {
    }
}