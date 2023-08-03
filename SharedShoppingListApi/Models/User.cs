using System.ComponentModel.DataAnnotations;

namespace SharedShoppingListApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string UniqueId { get; set; } = null!;
    }
}
