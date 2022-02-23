using LucidbrndClear.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucidbrndClear.Data.Interfaces
{
    public interface IAllProductsInCart
    {
        public List<(Product, string, int)> GetAllProductsFromCart(string CartId);
        public void CreateNewProductInCart(string _CartId, string ProductId, string size);
        public void RemoveFromCart(string CartId, string ProductId,string Size);
        public void RemoveWholeProductFromCart(string CartId, string ProductId, string Size);

        public bool CompareSize(Product Product, string CartId, string size);
    }
}
