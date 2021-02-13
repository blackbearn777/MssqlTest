using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MssqlTestApp.Entities
{
    class Order
    {
        public Order(string customerId, DateTime orderDate, string orderID, decimal sumOrd)
        {
            CustomerId = customerId;
            OrderDate = orderDate;
            OrderID = orderID;
            SumOrd = sumOrd;
        }

        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        [Key]
        public string OrderID { get; set; }
        public decimal  SumOrd { get; set; }
    }
}
