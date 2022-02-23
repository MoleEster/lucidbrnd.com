using LucidbrndClear.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucidbrndClear.Data.Interfaces
{
    public interface IAllOrders
    {
        public void AddOrderToDatabase(List<(Product, string, int)> Order, string Email, string Adress,string Phone, string Name,Guid OrderId);
        public Order GetOrderById(string Id);
        public void ChangeStatus(Order order);
    }
}
