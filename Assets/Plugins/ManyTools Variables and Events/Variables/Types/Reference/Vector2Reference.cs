using UnityEngine;

namespace ManyTools.Variables
{
    [System.Serializable]
    public class Vector2Reference : Reference<Vector2, Vector2Variable>
    {
        public Vector2Reference(Vector2 value) : base(value)
        {
        }
    }
}