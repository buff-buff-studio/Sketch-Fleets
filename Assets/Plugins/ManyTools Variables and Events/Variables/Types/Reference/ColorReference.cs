using UnityEngine;

namespace ManyTools.Variables
{
    [System.Serializable]
    public class ColorReference : Reference<Color, ColorVariable>
    {
        public ColorReference(Color value) : base(value)
        {
        }
    }
}