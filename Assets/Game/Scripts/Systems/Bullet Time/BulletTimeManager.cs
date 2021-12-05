using System.Collections;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// A class that manages bullet time
    /// </summary>
    public sealed class BulletTimeManager : MonoBehaviour
    {
        #region Private Fields

        private float _fixedDeltaTimeCache;
        private Coroutine _bulletTimeCoroutine;
        
        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            _fixedDeltaTimeCache = Time.fixedDeltaTime;
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Starts bullet time
        /// </summary>
        /// <param name="timeByRealTime">The time scale by real time graph of the bullet time</param>
        /// <param name="duration">The duration of the bullet time</param>
        public void StartBulletTime(AnimationCurve timeByRealTime, float duration)
        {
            StopBulletTime();

            _bulletTimeCoroutine = StartCoroutine(BulletTime(timeByRealTime, duration));
        }

        #endregion
        
        #region Private Methods

        /// <summary>
        /// Stops any ongoing bullet times
        /// </summary>
        private void StopBulletTime()
        {
            if (_bulletTimeCoroutine != null)
            {
                StopCoroutine(_bulletTimeCoroutine);
            }

            Time.timeScale = 1f;
            Time.fixedDeltaTime = _fixedDeltaTimeCache;
        }
        
        /// <summary>
        /// Stops controlling timeScale
        /// </summary>
        private void RelinquishTimeControl()
        {
            if (_bulletTimeCoroutine != null)
            {
                StopCoroutine(_bulletTimeCoroutine);
            }
            
            Time.fixedDeltaTime = _fixedDeltaTimeCache;
        }

        /// <summary>
        /// Slows down time to make a bullet-time-like effect
        /// </summary>
        /// <param name="timeByRealTime">The time by real time graph</param>
        /// <param name="duration">The duration of the bullet time</param>
        private IEnumerator BulletTime(AnimationCurve timeByRealTime, float duration)
        {
            WaitForSecondsRealtime waitForRealTime = new WaitForSecondsRealtime(duration / 20f);
            float lastTimeScale = Time.timeScale;
            
            for (int index = 0; index < 20; index++)
            {
                if (!Mathf.Approximately(Time.timeScale, lastTimeScale))
                {
                    RelinquishTimeControl();
                    yield break;
                }
                
                Time.timeScale = timeByRealTime.Evaluate(duration / 20f * index);
                lastTimeScale = Time.timeScale;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                yield return waitForRealTime;
            }
            
            StopBulletTime();
        }
        
        #endregion
    }
}
