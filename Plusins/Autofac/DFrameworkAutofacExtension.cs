namespace DFramework.Autofac
{
    public static class DFrameworkAutofacExtension
    { 
        /// <summary>
        /// use couchbase memacached
        /// </summary>
        /// <param name="framework"></param>
        /// <returns>FCFramework</returns>
        public static DEnvironment UseAutofac(this DEnvironment framework)
        {
            IoC.InitializeWith(new DependencyResolverFactory(typeof(AutofacDependencyResolver)));

            return framework;
        }
    }
}
