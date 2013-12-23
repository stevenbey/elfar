using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Elfar
{
    public static class ErrorLogProvider
    {
        internal static bool Delete(int id)
        {
            if(Instance != null)
            {
                try
                {
                    Instance.Delete(id);
                    return true;
                }
                catch(Exception) { }
            }
            return false;
        }
        internal static void Save(ErrorLog errorLog)
        {
            Save(errorLog, true);
        }

        static void Plugins(ErrorLog errorLog, bool save = true)
        {
            try { ErrorLogPlugins.Execute(errorLog); }
            catch(Exception e) { if(save) Save(e); }
        }
        static void Plugins(Exception exception)
        {
            Plugins(new ErrorLog(exception), false);
        }
        static void Save(ErrorLog errorLog, bool plugins)
        {
            if(plugins) Plugins(errorLog);
            
            if(Instance == null) return;
            
            try { Instance.Save(errorLog); }
            catch(Exception e) { Plugins(e); }
        }
        static void Save(Exception exception)
        {
            Save(new ErrorLog(exception), false);
        }

        public static Settings Settings
        {
            get { return settings ?? (settings = new Settings()); }
            set { settings = value; }
        }

        internal static IEnumerable<string> All
        {
            get
            {
                try
                {
                    var provider = Instance as IJsonErrorLogProvider;
                    if(provider != null) return provider.Json;
                    if(Instance != null) return Instance.All.Select(l => new ErrorLog.Storage(l).Json);
                }
                catch(Exception) {}
                return empty;
            }
        }
        internal static string Application
        {
            get
            {
                var path = HttpRuntime.AppDomainAppVirtualPath.Trim('/');
                return string.IsNullOrWhiteSpace(path) ? HttpRuntime.AppDomainAppId : path;
            }
        }
        internal static string Details
        {
            get { return Type == null ? null : string.Format("{0} [{1}]", Type.Namespace, Type.Assembly.GetName().Version); }
        }

        static IErrorLogProvider Instance
        {
            get { return instance ?? (instance = Components.Create<IErrorLogProvider>() ?? new Cache()); }
        }
        static Type Type
        {
            get { return Instance == null ? null : Instance.GetType(); }
        }

        static readonly string[] empty = new string[0];
        static IErrorLogProvider instance;
        static Settings settings;

        [PartNotDiscoverable]
        class Cache : Dictionary<int, ErrorLog.Storage>, IErrorLogProvider, IJsonErrorLogProvider
        {
            void IErrorLogProvider.Delete(int id)
            {
                Remove(id);
            }
            void IErrorLogProvider.Save(ErrorLog errorLog)
            {
                Add(errorLog.ID, new ErrorLog.Storage(errorLog));
            }

            IEnumerable<ErrorLog> IErrorLogProvider.All
            {
                get { throw new NotImplementedException(); }
            }
            IEnumerable<string> IJsonErrorLogProvider.Json
            {
                get { return Values.Select(l => l.Json); }
            }
        }
    }
}