using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucidbrndClear.Data.Models
{
    public class ProductInCart
    {
        public Guid ProductInCartId { get; set; }
        public string ProductId { get; set; }
        public int Amount { get; set; }
        public string Size { get; set; }
        public string CartId { get; set; }
    }
}
