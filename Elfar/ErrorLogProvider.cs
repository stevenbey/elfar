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
            catch(Exception e) { throw new ErrorLogException(e); }
        }
        public static void Save(ErrorLog errorLog)
        {
            if(Instance == null) return;
            try { Instance.Save(errorLog); }
            catch(Exception e) { throw new ErrorLogException(e); }
        }

        public static IEnumerable<ErrorLog> All
        {
            get
            {
                try { return Instance == null ? new ErrorLog[0] : Instance.All; }
                catch(Exception e) { throw new ErrorLogException(e); }
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