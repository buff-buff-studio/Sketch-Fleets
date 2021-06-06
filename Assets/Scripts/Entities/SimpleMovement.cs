using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    ///  A class that moves objects in 2D space
    /// </summary>
    public class SimpleMovement : MonoBehaviour
    {
        #region Private Fields

        [Header("Movement Configurations")]
        [SerializeField]
        private Vector2Reference backgroundSpeed;
        private Transform cachedTransform;
        
        #endregion

        #region Unity Callbacks

        // Start is called before the first frame update
        void Start()
        {
            cachedTransform = transform;
        }

        // Update is called once per frame
        void Update()
        {
            cachedTransform.Translate(backgroundSpeed.Value * (Time.deltaTime * Time.timeScale));        
        }

        #endregion
    }
}
