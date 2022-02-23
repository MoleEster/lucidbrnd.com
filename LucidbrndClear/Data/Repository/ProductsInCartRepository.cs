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
    public class ProductsInCartRepository : IAllProductsInCart
    {
        private AppDbContext appDbContext;
        public ProductsInCartRepository(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        public void CreateNewProductInCart(string _CartId, string ProductId,string size)
        {
            var AssumptionOfPresence = appDbContext.ProductsInCart.FirstOrDefault(p => p.CartId == _CartId && p.ProductId == ProductId && p.Size.ToLower() == size.ToLower());
            if (AssumptionOfPresence != null)
            {
                AssumptionOfPresence.Amount+=1;
                appDbContext.Entry(AssumptionOfPresence).State = EntityState.Modified;
            }
            else
            {
                ProductInCart NewProductInCart = new ProductInCart
                {
                    CartId = _CartId,
                    ProductId = ProductId,
                    ProductInCartId = Guid.NewGuid(),
                    Size = size,
                    Amount = 1
                };
                appDbContext.ProductsInCart.Add(NewProductInCart);
            }
            appDbContext.SaveChanges();
        }

        public List<(Product,string,int)> GetAllProductsFromCart(string CartId)
        {
            List<ProductInCart> AllProductsFromOneCart = appDbContext.ProductsInCart.Where(p => p.CartId == CartId).ToList();
            List<(Product, string, int)> CartItems = new List<(Product, string, int)>();
            foreach (ProductInCart Product in AllProductsFromOneCart)
            {
                CartItems.Add((appDbContext.Products.FirstOrDefault(p => string.Equals(p.ProductId.ToString(), Product.ProductId)),Product.Size,Product.Amount));
            }
            return CartItems;
        }

        public void RemoveFromCart(string CartId, string ProductId,string Size)
        {
            var selectedProduct = appDbContext.ProductsInCart.FirstOrDefault(p => p.ProductId == ProductId && p.CartId == CartId && p.Size.ToLower() == Size.ToLower());
            if (selectedProduct != null)
            {
                if (selectedProduct.Amount == 1)
                {
                    RemoveWholeProductFromCart(CartId, ProductId, Size);
                }
                else
                {
                    selectedProduct.Amount -= 1;
                    appDbContext.Entry(selectedProduct).State = EntityState.Modified;
                    appDbContext.SaveChanges();
                }
            }
        }

        public void RemoveWholeProductFromCart(string CartId, string ProductId, string Size)
        {
            var selectedProduct = appDbContext.ProductsInCart.FirstOrDefault(p => p.ProductId == ProductId && p.CartId == CartId && p.Size.ToLower() == Size.ToLower());
            if (selectedProduct != null)
            {
                appDbContext.Remove<ProductInCart>(selectedProduct);
                appDbContext.Entry(selectedProduct).State = EntityState.Deleted;
                try
                {
                  appDbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    string IDontKnowWhyIGetThisExeptionIfEverythingWorksWell = ex.Message;
                }
            }
        }


        public bool CompareSize(Product Product, string CartId, string size)
        {
            (Product, string, int) thisproductincart = GetAllProductsFromCart(CartId).FirstOrDefault(p => string.Equals(p.Item1.ProductId.ToString(), Product.ProductId.ToString()) && string.Equals(p.Item2,size));
            switch (size)
            {
                case "XS":
                    if (thisproductincart.Item1.AmountOfSizeXS - thisproductincart.Item3 > 0)
                        return true;
                    return false;
                case "S":
                    if (thisproductincart.Item1.AmountOfSizeS - thisproductincart.Item3 > 0)
                        return true;
                    return false;
                case "M":
                    if (thisproductincart.Item1.AmountOfSizeM - thisproductincart.Item3 > 0)
                        return true;
                    return false; 
                case "L":
                    if (thisproductincart.Item1.AmountOfSizeL - thisproductincart.Item3 > 0)
                        return true;
                    return false; 
                case "XL":
                    if (thisproductincart.Item1.AmountOfSizeXL - thisproductincart.Item3 > 0)
                        return true;
                    return false;
            }
            return false;
        }
    }
}
