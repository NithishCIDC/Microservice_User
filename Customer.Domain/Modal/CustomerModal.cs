using System.ComponentModel.DataAnnotations;

namespace Customer.Domain.Modal
{
    public class CustomerModal
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        //public ICollection<OrderModal> Orders { get; set; }

    }
}
