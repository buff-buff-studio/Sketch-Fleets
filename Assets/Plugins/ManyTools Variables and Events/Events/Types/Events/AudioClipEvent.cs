using UnityEngine;

namespace ManyTools.Events
{
    [CreateAssetMenu(fileName = CreateMenus.AudioClipEventFileName, menuName = CreateMenus.AudioClipEventMenu,
        order = CreateMenus.AudioClipEventOrder)]
    public class AudioClipEvent : GameEvent<AudioClip>
    {
        
    }
}