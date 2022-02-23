using LucidbrndClear.Data.Interfaces;
using LucidbrndClear.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucidbrndClear.Controllers
{
    public class SuccessController : Controller
    {
        private IAllOrders allOrders;
        private IAllProducts allProducts;
        private IAllUsers allUsers;
        private IAllProductsInCart allProductsInCart;

        public SuccessController(IAllOrders _allOrders, IAllProducts _allProducts,IAllUsers _allUsers, IAllProductsInCart _allProductsInCart)
        {
            allOrders = _allOrders;
            allProducts = _allProducts;
            allUsers = _allUsers;
            allProductsInCart = _allProductsInCart;
        }
        [Route("Success/{OrderId}")]
        public async Task<IActionResult> Index(string OrderId)
        {
            var thisOrder = allOrders.GetOrderById(OrderId);
            if(thisOrder != null)
            { 
                allOrders.ChangeStatus(thisOrder);
            }
            else
            {
                return View("Error");
            }
            var activeuser = Request.Cookies["ActiveUser"];
            User ThisUser = allUsers.GetUser(activeuser);
            await Task.Run(() => EmailService.SendEmailToCreatorsAsync(thisOrder.Email,thisOrder.Adress,thisOrder.Name,thisOrder.Phone,thisOrder.Products));
            //await Task.Run(() => EmailService.SendEmailToCustomer(thisOrder.Email, "check"));
            var order = allProductsInCart.GetAllProductsFromCart(ThisUser.CartId);
            allProducts.DecreaseAmountOProductsSize(order);
            foreach ((Product, string, int) item in order)
            {
                await Task.Run(() => allProductsInCart.RemoveWholeProductFromCart(ThisUser.CartId, item.Item1.ProductId.ToString(), item.Item2));
            }
            return View();
        }
    }
}
