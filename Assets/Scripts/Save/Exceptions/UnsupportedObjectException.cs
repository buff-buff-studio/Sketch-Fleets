using System;

namespace SketchFleets
{

    [Serializable]
    public class UnsupportedObjectException : Exception
    {
        public UnsupportedObjectException() : base() { }
        public UnsupportedObjectException(string message) : base(message) { }
        public UnsupportedObjectException(string message, Exception inner) : base(message, inner) { }
    }
}
