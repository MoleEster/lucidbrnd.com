using LucidbrndClear.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucidbrndClear.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<(Product,string,int)> UsersProductsInCart { get; set; }

        public double SumPrice
        {
            get
            {
                double sumrpice = 0;
                foreach (var item in UsersProductsInCart)
                {
                    sumrpice += item.Item1.Price*item.Item3;
                }
                return sumrpice;
            }
        }
    }
}
