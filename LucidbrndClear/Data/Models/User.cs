using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucidbrndClear.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public DateTime LoginDate { get; set; }
        public string CartId { get; set; }
    }
}
