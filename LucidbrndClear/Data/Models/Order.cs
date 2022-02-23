using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucidbrndClear.Data.Models
{
    public class Order
    {
        public Order(string products, string email, string adress, string name,string phone)
        {
            Products = products;
            Email = email;
            Adress = adress;
            Name = name;
            Phone = phone;
        }

        public Guid OrderId { get; set; }
        public string Products { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}
