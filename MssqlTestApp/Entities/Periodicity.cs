using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MssqlTestApp.Entities
{
    class Periodicity
    {
        public Periodicity(string customerId, int days)
        {
            CustomerId = customerId;
            Days = days;
        }

        [Key]
        public string CustomerId { get; set; }
        public int Days { get; set; }
    }
}
