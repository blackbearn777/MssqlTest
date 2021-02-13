using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MssqlTestApp.Entities
{
    
    class Holiday
    {
        public Holiday(DateTime dateH)
        {
            DateH = dateH;
        }
        [Key]
        public DateTime DateH { get; set; }
    }
}
