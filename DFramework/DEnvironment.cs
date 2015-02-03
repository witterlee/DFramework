using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFramework
{
    public class DEnvironment
    {
        public static DEnvironment Instance { get; private set; }
         

        //public static ICommandBus DefaultCommandBus { get { return IoC.Resolve<ICommandBus>(); } }

        private DEnvironment() { }

        public static DEnvironment Initialize()
        {
            if (Instance != null)
            {
                throw new Exception("Could not initialize twice");
            }

            Instance = new DEnvironment();                                        

            return Instance;
        }

        public void Start()
        {
            IoC.Register<IJsonSerializer, JsonSerializer>(LifeStyle.Singleton);
        }
    }
}
