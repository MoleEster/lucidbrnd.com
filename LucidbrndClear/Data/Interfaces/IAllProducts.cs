using LucidbrndClear.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucidbrndClear.Data.Interfaces
{
    public interface IAllProducts
    {
        public IEnumerable<Product> GetAllProducts { get; }
        public Product GetProductByName(string Name);
        public Product GetProductById(string Id);
        public bool DecreaseAmountOProductsSize(List<(Product, string, int)> order);
        public bool CompareSizes(string ProductId, string size);
    }
}
