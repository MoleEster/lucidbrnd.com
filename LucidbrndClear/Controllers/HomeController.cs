using LucidbrndClear.Data.Interfaces;
using LucidbrndClear.Data.Models;
using LucidbrndClear.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LucidbrndClear.Controllers
{
    public class HomeController : Controller
    {
        private IAllProducts allProducts;
        private IAllProductsInCart allProductsInCart;
        private IAllUsers allUsers;

        public HomeController(IAllProducts _allProducts, IAllUsers _allUsers, IAllProductsInCart _allProductsInCart)
        {
            allProducts = _allProducts;
            allUsers = _allUsers;
            allProductsInCart = _allProductsInCart;
        }

        public IActionResult Index(bool? ShowAlert = false)
        {
            // Идентификация пользователя
            int ThisUserCart = 0;
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
                else
                {
                    // Подсчет количества товаров в корзине
                    foreach (int item in allProductsInCart.GetAllProductsFromCart(ActiveUser.CartId).Select(p => p.Item3))
                    {
                        ThisUserCart += item;
                    }
                }
            }

            var view = new HomeViewModel
            {
                Products = allProducts.GetAllProducts,
                ItemsInCart = ThisUserCart,
                ShowAlert = (bool)ShowAlert
            };
            ShowAlert = false;
            return View(view);
        }
        [HttpPost]
        public IActionResult GetProduct(string ProductId)
        {
            // Поиск отдельного продукта для показа
            if (ProductId != null)
                return PartialView("ProductPopup", new HomeViewModel { Product = allProducts.GetProductById(ProductId) });
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(string ProductId, string size)
        {
            var activeuser = Request.Cookies["ActiveUser"];
            User ThisUser = allUsers.GetUser(activeuser);
            if (ProductId != null && size != null)
            {
                // Проверка наличия размеров в принципе
                if (allProducts.CompareSizes(ProductId, size))
                {
                    // Проверка на то, чтобы количество заказываемых единиц товара не превышало количество товара в наличии
                    if (allProductsInCart.GetAllProductsFromCart(ThisUser.CartId).FirstOrDefault(p => string.Equals(p.Item1.ProductId.ToString(), ProductId) && string.Equals(p.Item2, size)).Item1 == null)
                    {
                        await Task.Run(() => allProductsInCart.CreateNewProductInCart(ThisUser.CartId, ProductId, size));
                    }
                    else
                    {
                        if (allProductsInCart.CompareSize(allProducts.GetProductById(ProductId), ThisUser.CartId, size))
                        {
                            await Task.Run(() => allProductsInCart.CreateNewProductInCart(ThisUser.CartId, ProductId, size));
                        }
                    }
                }
                return RedirectToAction("Index", new { ShowAlert = true }) ;
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> TellAboutError()
        {
            // В случае непредвиденной ошибки
            await Task.Run(() => EmailService.SendErrorEmail());
            return RedirectToAction("Index");
        }
    }
}
