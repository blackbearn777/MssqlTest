using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MssqlTestApp.Entities
{
    class PaySum
    {
        public PaySum(string customerId, decimal pay)
        {
            CustomerId = customerId;
            Pay = pay;
        }

        [Key]
        public string CustomerId { get; set; }
        public decimal  Pay { get; set; }
    }
}
