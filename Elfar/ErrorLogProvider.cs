using System;
using System.ComponentModel;
using System.Linq;

namespace Elfar
{
    // ReSharper disable EmptyGeneralCatchClause
    // ReSharper disable ParameterHidesMember
    public static class ErrorLogProvider
    {
        static ErrorLogProvider()
        {
            Instance = Composition.CreateMany<IErrorLogProvider>().FirstOrDefault();
            plugins = Composition.CreateMany<IErrorLogPlugin>().ToArray();
            if (Settings != null) application = Settings.Application;
            if (string.IsNullOrWhiteSpace(application)) application = Settings.AppDomainAppVirtualPath ?? Settings.AppDomainAppId;
        }
        
        internal static bool Delete(Guid id)
        {
            try
            {
                Instance.Delete(id);
                return true;
            }
            catch { }
            return false;
        }
        internal static string Get(Guid id)
        {
            return Instance[id];
        }
        internal static void Save(ErrorLog errorLog, bool plugins = true)
        {
            if (plugins) Plugins(errorLog);
            if (Instance == null) return;
            try { Instance.Save(errorLog); }
            catch (Exception e) { Plugins(new ErrorLog(e), false); }
        }
        internal static bool Save(Guid id, string detail)
        {
            try
            {
                Instance[id] = detail;
                return true;
            }
            catch { }
            return false;
        }

        static void Plugins(ErrorLog errorLog, bool save = true)
        {
            try { Array.ForEach(plugins, p => p.Execute(errorLog)); }
            catch (Exception e) { if (save) Save(new ErrorLog(e), false); }
        }

        public static string Application
        {
            get { return application; }
        }
        public static string Name
        {
            get
            {
                var type = Instance.GetType();
                var attribute = (DisplayNameAttribute) type.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault();
                return attribute == null ? type.Name.Replace("ErrorLogProvider", "") : attribute.DisplayName;
            }
        }
        public static Settings Settings { get; set; }

        internal static IErrorLogProvider Instance { get; private set; }
        internal static string Summaries
        {
            get { return Instance.Summaries; }
        }

        private static readonly string application;
        static readonly IErrorLogPlugin[] plugins;
    }
}