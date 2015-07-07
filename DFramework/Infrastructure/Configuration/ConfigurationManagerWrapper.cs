
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;

namespace DFramework
{
    public class ConfigurationManagerWrapper
    {
        public static NameValueCollection AppSettings
        {
            [DebuggerStepThrough]
            get { return ConfigurationManager.AppSettings; }
        }
        [DebuggerStepThrough]
        public static string GetDBConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        [DebuggerStepThrough]
        public static string GetMessageQueueServerConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
        [DebuggerStepThrough]
        public static string GetProviderName(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ProviderName;
        }
        [DebuggerStepThrough]
        public static T GetSection<T>(string sectionName)
        {
            return (T)ConfigurationManager.GetSection(sectionName);
        }
    }
}
