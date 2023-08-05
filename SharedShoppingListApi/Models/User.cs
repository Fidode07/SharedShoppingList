using System.ComponentModel.DataAnnotations;

namespace SharedShoppingListApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string UniqueId { get; set; } = string.Empty;
        public ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
