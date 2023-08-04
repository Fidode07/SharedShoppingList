using System.ComponentModel.DataAnnotations;

namespace SharedShoppingListApi.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<User> Members { get; set; } = new List<User>();
        public string ShoppingList { get; set; } = string.Empty;
    }
}
