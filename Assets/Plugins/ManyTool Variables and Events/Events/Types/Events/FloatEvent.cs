using UnityEngine;

namespace ManyTools.Events
{
    [CreateAssetMenu(fileName = CreateMenus.FloatEventFilename, menuName = CreateMenus.FloatEventMenu,
        order = CreateMenus.FloatEventOrder)]
    public class FloatEvent : GameEvent<float>
    {
        
    }
}