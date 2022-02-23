using LucidbrndClear.Data.Interfaces;
using LucidbrndClear.Data.Models;
using LucidbrndClear.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TinkoffPaymentClientApi;
using TinkoffPaymentClientApi.Commands;
using TinkoffPaymentClientApi.ResponseEntity;

namespace LucidbrndClear.Controllers
{
    public class CartController : Controller
    {
        private IAllProductsInCart allProductsInCart;
        private IAllProducts allProducts;
        private IAllUsers allUsers;
        private IAllOrders allOrders;

        public CartController(IAllUsers _allUsers, IAllProductsInCart _allProductsInCart, IAllOrders _allOrders, IAllProducts _allProducts)
        {
            allUsers = _allUsers;
            allProductsInCart = _allProductsInCart;
            allOrders = _allOrders;
            allProducts = _allProducts;
        }
        public IActionResult Index()
        {
            // идентификация пользователя
            User ActiveUser;
            var activeuser = Request.Cookies["ActiveUser"];
            if (activeuser == null)
            {
                ActiveUser = new User
                {
                    Id = Guid.NewGuid(),
                    CartId = Guid.NewGuid().ToString(),
                    LoginDate = DateTime.Now
                };
                Response.Cookies.Append("ActiveUser", ActiveUser.Id.ToString());
                allUsers.AddNewUser(ActiveUser);
            }
            else
            {
                // Создание нового пользователя и добавление его в базу
                ActiveUser = allUsers.GetUser(activeuser);
                if (ActiveUser == null)
                {
                    ActiveUser = new User
                    {
                        Id = Guid.NewGuid(),
                        CartId = Guid.NewGuid().ToString(),
                        LoginDate = DateTime.Now
                    };
                    Response.Cookies.Append("ActiveUser", ActiveUser.Id.ToString());
                    allUsers.AddNewUser(ActiveUser);
                }
            }
            var view = new CartViewModel
            {
                UsersProductsInCart = allProductsInCart.GetAllProductsFromCart(ActiveUser.CartId)
            };
            return View(view);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(string ProductId, string Size)
        {
            var activeuser = Request.Cookies["ActiveUser"];
            User ActiveUser = allUsers.GetUser(activeuser);
            if (ProductId != null)
            {
                await Task.Run(() => allProductsInCart.RemoveFromCart(ActiveUser.CartId, ProductId, Size));
            }
            return PartialView("ItemsInCart", allProductsInCart.GetAllProductsFromCart(ActiveUser.CartId));
        }

        [HttpPost]
        public async Task<IActionResult> AddtoCart(string ProductId, string Size)
        {
            var activeuser = Request.Cookies["ActiveUser"];
            User ThisUser = allUsers.GetUser(activeuser);
            if (ProductId != null && Size != null)
            {
                // Проверка на не превышения заказываемого количества над количеством в наличии
                if (allProductsInCart.CompareSize(allProducts.GetProductById(ProductId),ThisUser.CartId, Size))
                {
                    await Task.Run(() => allProductsInCart.CreateNewProductInCart(ThisUser.CartId, ProductId, Size));
                }
                return PartialView("ItemsInCart", allProductsInCart.GetAllProductsFromCart(ThisUser.CartId));
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return View();
            }

        }

        [HttpPost]
        public async Task<IActionResult> RemoveWholeProductInCart(string ProductId, string Size)
        {
            var activeuser = Request.Cookies["ActiveUser"];
            User ThisUser =  allUsers.GetUser(activeuser);
            if (ProductId != null && Size != null)
            {
                await Task.Run(() => allProductsInCart.RemoveWholeProductFromCart(ThisUser.CartId, ProductId, Size));
                return PartialView("ItemsInCart", allProductsInCart.GetAllProductsFromCart(ThisUser.CartId));
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return View();
            }
        }

        [HttpPost]
        public IActionResult CreateNewSuccesfulOrder(string Email,string FIO,string Phone,string Adress)
        {
            var activeuser = Request.Cookies["ActiveUser"];
            User ThisUser = allUsers.GetUser(activeuser);
            if (ThisUser != null && Email !=null && FIO !=null && Phone !=null && Adress !=null)
            {
                uint SummaryPrice = 30000;
                List<(Product,string,int)> order = allProductsInCart.GetAllProductsFromCart(ThisUser.CartId);
                // Подсчет суммы оплаты
                foreach (var item in order)
                {
                    SummaryPrice += (uint)item.Item1.Price * (uint)item.Item3 * 100;
                }
                // Выделение памяти для клиента оплаты
                var OrderId = Guid.NewGuid();
                Init
                    init = new Init(Guid.NewGuid().ToString(),SummaryPrice);
                init.SetEmail(Email);
                init.SetPhone(Phone);
                init.IP = SummaryPrice.ToString();
                init.SuccessURL = @$"https://lucidbrnd.com/Success/{OrderId}";

                var client = new TinkoffPaymentClient("1632515708685", "xt6f9uxpqtvzafu8");
                string json = JsonSerializer.Serialize<Init>(init);
                var s = client.Init(init).Details;
                PaymentResponse newInit = client.Init(init);
                allOrders.AddOrderToDatabase(order, Email, Adress,Phone,FIO,OrderId);
                var uri = new Uri(newInit.PaymentURL);
                return Redirect(uri.ToString());
            }
            
            return RedirectToAction("Index", "Cart");
        }
    }
}
