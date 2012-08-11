using System;
using System.Collections.Generic;
using System.Linq;

namespace Elfar
{
    public class ErrorLogProvider
        : Dictionary<Guid, ErrorLog>,
          IErrorLogProvider
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

        public string Application { get; set; }
    }
}