using LucidbrndClear.Data.Database;
using LucidbrndClear.Data.Interfaces;
using LucidbrndClear.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucidbrndClear.Data.Repository
{
    public class ProductsRepository : IAllProducts
    {
        private AppDbContext appDbContext;
        public ProductsRepository(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        public IEnumerable<Product> GetAllProducts => appDbContext.Products.ToList();


        public bool DecreaseAmountOProductsSize(List<(Product,string,int)> order)
        {
            List<string> productInCart = order.Select(p=>p.Item1.ProductId.ToString()).Distinct().ToList();
            if (order.Count() != 0)
            {
                foreach (string item in productInCart.Distinct())
                {
                    Product changedProdut = appDbContext.Products.FirstOrDefault(p => string.Equals(p.ProductId.ToString(), item));


                    foreach ((Product, string, int) oneproduct in order.Where(p=> string.Equals(p.Item1.ProductId.ToString(), changedProdut.ProductId.ToString())))
                    {
                        switch (oneproduct.Item2)
                         {
                             case "XS":
                                changedProdut.AmountOfSizeXS-= oneproduct.Item3;
                                 break;
                             case "S":
                                changedProdut.AmountOfSizeS -= oneproduct.Item3;
                                 break;
                             case "M":
                                changedProdut.AmountOfSizeM -= oneproduct.Item3;
                                 break;
                             case "L":
                                changedProdut.AmountOfSizeL -= oneproduct.Item3;
                                 break;
                             case "XL":
                                changedProdut.AmountOfSizeXL -= oneproduct.Item3;
                                 break;
                         }
                    }
                    appDbContext.Entry(changedProdut).State = EntityState.Modified;

                }
                appDbContext.SaveChanges();       
                return true;
            }
            else
                return false;
        }

        public Product GetProductById(string Id) => appDbContext.Products.FirstOrDefault(p => string.Equals(p.ProductId.ToString(), Id));

        public Product GetProductByName(string Name) => appDbContext.Products.FirstOrDefault(p => string.Equals(p.Name, Name,StringComparison.OrdinalIgnoreCase));
        public bool CompareSizes(string ProductId, string size)
        {
            Product thisproductincart = GetProductById(ProductId);
            switch (size)
            {
                case "XS":
                    if (thisproductincart.AmountOfSizeXS >= 1)
                        return true;
                    return false;
                case "S":
                    if (thisproductincart.AmountOfSizeS >= 1)
                        return true;
                    return false;
                case "M":
                    if (thisproductincart.AmountOfSizeM >= 1)
                        return true;
                    return false;
                case "L":
                    if (thisproductincart.AmountOfSizeL >= 1)
                        return true;
                    return false;
                case "XL":
                    if (thisproductincart.AmountOfSizeXL >= 1)
                        return true;
                    return false;
            }
            return false;
        }
    }
}
