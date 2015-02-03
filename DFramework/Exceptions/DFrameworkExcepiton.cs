using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFramework
{
    public class DFrameworkExcepiton : SystemException
    {
        public DFrameworkExcepiton(string message) : base(message) { }

        public DFrameworkExcepiton(string message, Exception innerException) : base(message, innerException) { }

    }
}
