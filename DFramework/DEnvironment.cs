using System;

namespace DFramework
{
    public class DEnvironment
    {
        public static DEnvironment Instance { get; private set; }

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

        public DEnvironment UseDefaultJsonSerializer()
        {
            IoC.Register<IJsonSerializer, JsonSerializer>(LifeStyle.Singleton);
            return this;
        }
    }
}
