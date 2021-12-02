using ManyTools.Variables;
using SketchFleets.Data;

namespace SketchFleets.Variables
{
    [System.Serializable]
    public class BulletAttributesReference : Reference<BulletAttributes, BulletAttributesVariable>
    {
        public BulletAttributesReference(BulletAttributes value) : base(value)
        {
        }
    }
}