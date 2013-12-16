using System.Collections.Generic;
using System.Linq;

namespace Elfar.Data
{
    public class ErrorLogProvider : Dictionary<int, ErrorLog>, IErrorLogProvider
    {
        public void Delete(int id)
        {
            Remove(id);
        }
        public void Save(ErrorLog errorLog)
        {
            Add(errorLog.ID, errorLog);
        }
        
        public IEnumerable<ErrorLog> All
        {
            get { return this.Select(p => p.Value); }
        }
    }
}