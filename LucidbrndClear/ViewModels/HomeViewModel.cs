using LucidbrndClear.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucidbrndClear.ViewModels
{
    public class HomeViewModel
    {
        public int ItemsInCart { get; set; }
        public IEnumerable<Product> Products;
        public Product Product { get; set; }
        public bool ShowAlert { get; set; }
    }
}
