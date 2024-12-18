using System.ComponentModel.DataAnnotations;

namespace User.Domain.Modal
{
    public class UserModal
    {
        [Key]
        public int CustomerId { get; set; }
        public required string CustomerName { get; set; }
        public string Role { get; set; } = "User";
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
