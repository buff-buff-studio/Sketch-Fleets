using UnityEngine;

namespace ManyTools.Variables
{
    [System.Serializable]
    public class Vector3Reference : Reference<Vector3, Vector3Variable>
    {
        public Vector3Reference(Vector3 value) : base(value)
        {
        }
    }
}