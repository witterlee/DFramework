using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DFramework.Utilities
{
    public class ConfigurationManagerWrapper
    {
        public static NameValueCollection AppSettings
        {
            [DebuggerStepThrough]
            get { return ConfigurationManager.AppSettings; }
        }
        [DebuggerStepThrough]
        public static string GetConnectionString(string name)
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
