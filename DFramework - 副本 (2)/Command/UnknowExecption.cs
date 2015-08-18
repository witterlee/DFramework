using System;

namespace DFramework
{
    [Serializable]
    public class UnknowExecption : SystemException
    {
        public UnknowExecption(string message) : base(message) { }

        public UnknowExecption(string message, Exception innerException) : base(message, innerException) { }
    }
}
