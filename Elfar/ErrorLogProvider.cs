using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;

namespace Elfar
{
    // ReSharper disable EmptyGeneralCatchClause
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
                catch { }
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
        public static string Name
        {
            get
            {
                if(Instance == null) return null;
                var type = Instance.GetType();
                var attr = (DisplayNameAttribute)type.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault();
                return attr == null ? type.Name : attr.DisplayName;
            }
        }

        internal static IEnumerable<ErrorLog.Storage> All
        {
            get
            {
                try { return Instance as IStorageProvider ?? Instance.Select(l => new ErrorLog.Storage(l)); }
                catch { return empty; }
            }
        }

        static IErrorLogProvider Instance
        {
            get { return instance ?? (instance = Composition.CreateMany<IErrorLogProvider>().FirstOrDefault() ?? new MemoryErrorLogProvider()); }
        }

        static readonly ErrorLog.Storage[] empty = new ErrorLog.Storage[0];
        static IErrorLogProvider instance;
        static Settings settings;

        [PartNotDiscoverable, DisplayName("Memory")]
        class MemoryErrorLogProvider : Dictionary<int, ErrorLog.Storage>, IErrorLogProvider, IStorageProvider
        {
            void IErrorLogProvider.Delete(int id)
            {
                Remove(id);
            }
            IEnumerator<ErrorLog> IEnumerable<ErrorLog>.GetEnumerator()
            {
                throw new NotImplementedException();
            }
            IEnumerator<ErrorLog.Storage> IEnumerable<ErrorLog.Storage>.GetEnumerator()
            {
                return Values.GetEnumerator();
            }
            void IErrorLogProvider.Save(ErrorLog errorLog)
            {
                Add(errorLog.ID, new ErrorLog.Storage(errorLog));
            }
        }
    }
}