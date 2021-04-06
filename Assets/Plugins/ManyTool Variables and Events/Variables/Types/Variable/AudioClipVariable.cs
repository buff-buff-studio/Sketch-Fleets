using UnityEngine;

namespace ManyTools.Variables
{
    [CreateAssetMenu(fileName = CreateMenus.AudioClipVariableFileName, 
        menuName = CreateMenus.AudioClipVariableMenu, order = CreateMenus.AudioClipVariableOrder)]
    public class AudioClipVariable : Variable<AudioClip>
    {
    }
}