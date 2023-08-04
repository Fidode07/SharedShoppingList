using SharedShoppingListApi.Models;

namespace SharedShoppingListApi.Dtos
{
    public class GroupsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
