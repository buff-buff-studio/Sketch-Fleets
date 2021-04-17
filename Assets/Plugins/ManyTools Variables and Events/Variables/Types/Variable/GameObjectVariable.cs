using UnityEngine;

namespace ManyTools.Variables
{
    [CreateAssetMenu(fileName = CreateMenus.GameObjectVariableFileName, 
        menuName = CreateMenus.GameObjectVariableMenu, order = CreateMenus.GameObjectVariableOrder)]
    public class GameObjectVariable : Variable<GameObject>
    {
    }
}