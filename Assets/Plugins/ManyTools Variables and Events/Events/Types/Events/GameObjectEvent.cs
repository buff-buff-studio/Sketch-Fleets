using UnityEngine;

namespace ManyTools.Events
{
    [CreateAssetMenu(fileName = CreateMenus.GameObjectEventFilename, menuName = CreateMenus.GameObjectEventMenu,
        order = CreateMenus.GameObjectEventOrder)]
    public class GameObjectEvent : GameEvent<GameObject>
    {
        
    }
}