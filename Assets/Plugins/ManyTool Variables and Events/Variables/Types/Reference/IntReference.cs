namespace ManyTools.Variables
{
    [System.Serializable]
    public class IntReference : Reference<int, IntVariable>
    {
        public IntReference(int value) : base(value)
        {
        }
    }
}