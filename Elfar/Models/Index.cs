using System;
using System.Collections.Generic;

namespace Elfar.Models
{
    public class Index
    {
        public Index(int page, int size)
        {
            Number = page;
            Size = size;
        }

        public string Application { get; set; }
        public IEnumerable<ErrorLog> Errors { get; set; }
        public int Number { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
        public int From { get { return (Number - 1) * Size + 1; } }
        public int To { get { return Math.Min(Number * Size, Total); } }
        public int Pages { get { return (int) Math.Ceiling((decimal) (Total / Size)) + 1; } }
        public int Next { get { return Number + 1; } }
        public int Previous { get { return Number - 1; } }
    }
}