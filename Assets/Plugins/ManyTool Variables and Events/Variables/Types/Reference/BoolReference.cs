namespace ManyTools.Variables
{
    [System.Serializable]
    public class BoolReference : Reference<bool, BoolVariable>
    {
        public BoolReference(bool value) : base(value)
        {
        }
    }
}