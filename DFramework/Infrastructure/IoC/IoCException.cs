using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFramework
{
    [Serializable]
    public class IoCException : DFrameworkExcepiton
    {
        public IoCException(string message) : base(message) { }

        public IoCException(string message, Exception innerException) : base(message, innerException) { }

        public IoCException(Exception innerException) : base("IoC Failed,see inner exception for detail.", innerException) { }
    }
}
