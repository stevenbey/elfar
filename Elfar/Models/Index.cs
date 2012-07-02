using System.Collections.Generic;

namespace Elfar.Models
{
    public class Index
    {
        public string Application { get; set; }
        public IEnumerable<ErrorLog> Errors { get; set; }
    }
}