using System;
using System.Collections.Generic;
using System.Linq;

namespace Elfar
{
    public class ErrorLogProvider
        : Dictionary<Guid, ErrorLog>,
          IErrorLogProvider
    {
        public ErrorLog Get(Guid id)
        {
            return ContainsKey(id) ? this[id] : null;
        }
        public IList<ErrorLog> List()
        {
            return this.Select(p => p.Value).ToList();
        }
        public void Save(ErrorLog errorLog)
        {
            Add(errorLog.ID, errorLog);
            Total++;
        }

        public string Application { get; set; }
        public int Total { get; private set; }
    }
}