using System;

namespace SketchFleets.SaveSystem
{
    [Serializable]
    public class UnsupportedOperationException : Exception
    {
        public UnsupportedOperationException() : base() { }
        public UnsupportedOperationException(string message) : base(message) { }
        public UnsupportedOperationException(string message, Exception inner) : base(message, inner) { }
    }
}
