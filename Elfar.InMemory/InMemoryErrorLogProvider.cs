using System;
using System.Collections.Generic;
using System.Linq;

namespace Elfar.InMemory
{
    public class InMemoryErrorLogProvider : Dictionary<Guid, ErrorLog>, IErrorLogProvider
    {
        public void Delete(Guid id)
        {
            Remove(id);
        }
        public ErrorLog Get(Guid id)
        {
            return ContainsKey(id) ? this[id] : null;
        }
        public IList<ErrorLog> List()
        {
            return this.Select(p => p.Value).OrderByDescending(e => e.Time).ToList();
        }
        public void Save(ErrorLog errorLog)
        {
            Add(errorLog.ID, errorLog);
        }

        public string Application
        {
            get { return ErrorLogProvider.Settings.Application; }
        }
    }
}