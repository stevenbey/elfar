using System;
using System.Collections.Generic;

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
        internal static void Save(Json json)
        {
            Save(new ErrorLog(json), true);
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

        internal static IEnumerable<ErrorLog> All
        {
            get
            {
                try { if(Instance != null) return Instance.All; }
                catch(Exception) {}
                return empty;
            }
        }
        internal static string Name
        {
            get { return Type == null ? null : Type.FullName; }
        }
        internal static string Version
        {
            get { return Type == null ? null : Type.Assembly.GetName().Version.ToString(); }
        }
        
        static IErrorLogProvider Instance
        {
            get { return instance ?? (instance = Components.Create<IErrorLogProvider>()); }
        }
        static Type Type
        {
            get { return Instance == null ? null : Instance.GetType(); }
        }

        static readonly ErrorLog[] empty = new ErrorLog[0];
        static IErrorLogProvider instance;
        static Settings settings;
    }
}