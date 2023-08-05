using SharedShoppingListApi.Models;

namespace SharedShoppingListApi.Dtos
{
    public class GroupDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<GroupMemberDto> Members { get; set; } = new List<GroupMemberDto>();
        public string ShoppingList { get; set; } = string.Empty;
    }
}
