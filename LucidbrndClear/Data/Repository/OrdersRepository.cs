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
    public class OrdersRepository: IAllOrders
    {
        private AppDbContext appDbContext;
        public OrdersRepository(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        public void AddOrderToDatabase(List<(Product, string, int)> Order, string Email, string Adress,string Phone, string Name,Guid OrderId)
        {
            string ThisProducts = null ;
            foreach ((Product, string, int) item in Order)
            {
                ThisProducts += $"/{item.Item1.Name}, {item.Item2}, {item.Item3}//";
            }
            Order ThisOrder = new Order(ThisProducts, Email, Adress, Name,Phone);
            ThisOrder.OrderId = OrderId;
            ThisOrder.Status = "Не оплачен";
            appDbContext.Orders.Add(ThisOrder);
            appDbContext.SaveChanges();
        }

        public Order GetOrderById(string Id) => appDbContext.Orders.FirstOrDefault(o => string.Equals(o.OrderId.ToString(), Id));
        public void ChangeStatus(Order order)
        {
            order.Status = "Оплачен";
            appDbContext.Entry(order).State = EntityState.Modified;
            appDbContext.SaveChanges();
        }
    }
}
