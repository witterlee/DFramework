using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFramework
{
    public enum LifeStyle
    {
        Transient,
        Singleton,
        PerHttpRequest,
        PerThread
    }
}
