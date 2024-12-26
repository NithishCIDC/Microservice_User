using System.ComponentModel.DataAnnotations;

namespace User.Domain.Modal
{
    public class BaseModal
    {
        [Key]
        public int UserId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}
