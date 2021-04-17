using UnityEngine;

namespace ManyTools.Events
{
    [CreateAssetMenu(fileName = CreateMenus.AnimationCurveEventFilename, 
        menuName = CreateMenus.AnimationCurveEventMenu, order = CreateMenus.AnimationCurveEventOrder)]
    public class AnimationCurveEvent : GameEvent<AnimationCurve>
    {
    }
}