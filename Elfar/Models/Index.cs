using System.Collections.Generic;

namespace Elfar.Models
{
    public class Index
    {
        public string Application { get; set; }
        public IList<ErrorLog> Errors { get; set; }
        public IErrorLogPlugin[] Plugins { get; set; }
    }
}