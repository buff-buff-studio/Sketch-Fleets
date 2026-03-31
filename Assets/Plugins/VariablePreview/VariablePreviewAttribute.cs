using UnityEngine;

namespace SketchFleets.Plugins
{
    public class VariablePreviewAttribute : PropertyAttribute
    {
        public float height;
        public VariablePreviewAttribute(float height = 64f)
        {
            this.height = height;
        }
    }
}