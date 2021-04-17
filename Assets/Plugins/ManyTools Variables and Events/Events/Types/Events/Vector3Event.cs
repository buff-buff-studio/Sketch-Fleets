using UnityEngine;

namespace ManyTools.Events
{
    [CreateAssetMenu(fileName = CreateMenus.Vector3EventFilename, menuName = CreateMenus.Vector3EventMenu,
        order = CreateMenus.Vector3EventOrder)]
    public class Vector3Event : GameEvent<Vector3>
    {
        
    }
}