using SketchFleets.General;
using UnityEngine;

namespace SketchFleets.Systems
{
    /// <summary>
    /// A class that causes bullet time when spawned
    /// </summary>
    public sealed class BulletTimeCause : MonoBehaviour
    {
        #region Private Fields

        [Header("Bullet Time Parameters")]
        [SerializeField]
        private AnimationCurve timeByRealTime = new AnimationCurve();
        [SerializeField]
        private float bulletTimeDuration = 1.0f;

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            LevelManager.Instance.BulletTimeManager.StartBulletTime(timeByRealTime, bulletTimeDuration);
        }

        #endregion
    }
}