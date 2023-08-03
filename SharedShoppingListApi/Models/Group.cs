using System.ComponentModel.DataAnnotations;

namespace SharedShoppingListApi.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<User> Members { get; set; } = null!;
        public string ShoppingList { get; set; } = null!;
    }
}
