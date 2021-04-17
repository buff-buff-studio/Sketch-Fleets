using UnityEngine;

namespace ManyTools.Events
{
    [CreateAssetMenu(fileName = CreateMenus.Vector4EventFilename, menuName = CreateMenus.Vector4EventMenu,
        order = CreateMenus.Vector4EventOrder)]
    public class Vector4Event : GameEvent<Vector4>
    {
        
    }
}