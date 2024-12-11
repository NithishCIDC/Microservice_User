using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Customer.Domain.Modal
{
    public class OrderModal
    {
        [Key]
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string ProductName { get; set; }
        [JsonIgnore]
        public CustomerModal Customer { get; set; }
    }
}
