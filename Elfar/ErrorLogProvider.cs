using System;
using System.Collections.Generic;

namespace Elfar
{
    public static class ErrorLogProvider
    {
        public static void Delete(int id)
        {
            if(Instance == null) return;
            try { Instance.Delete(id); }
            catch(Exception) { }
        }
        public static void Save(ErrorLog errorLog)
        {
            if(Instance == null) return;
            try { Instance.Save(errorLog); }
            catch(Exception) {}
        }

        public static IEnumerable<ErrorLog> All
        {
            get
            {
                try { if(Instance != null) return Instance.All; }
                catch(Exception) {}
                return new ErrorLog[0];
            }
        }
        public static Settings Settings
        {
            get { return settings ?? (settings = new Settings()); }
            set { settings = value; }
        }
        public static string Version
        {
            get { return Instance.GetType().Assembly.GetName().Version.ToString(); }
        }
        
        static IErrorLogProvider Instance
        {
            get { return instance ?? (instance = Components.Create<IErrorLogProvider>()); }
        }

        static IErrorLogProvider instance;
        static Settings settings;
    }
}