using System.Collections.Generic;

namespace Elfar.Mvc.Models
{
    public class Index
    {
        public string Application { get; set; }
        public IList<Elfar.ErrorLog> Errors { get; set; }
        public IErrorLogPlugin[] Plugins { get; set; }
    }
}