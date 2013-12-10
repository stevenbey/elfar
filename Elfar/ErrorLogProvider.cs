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
        public static Type Type
        {
            get { return Instance.GetType(); }
        }
        public static string Version
        {
            get { return Type.Assembly.GetName().Version.ToString(); }
        }
        
        static IErrorLogProvider Instance
        {
            get { return instance ?? (instance = Components.Create<IErrorLogProvider>()); }
        }

        static IErrorLogProvider instance;
        static Settings settings;
    }
}