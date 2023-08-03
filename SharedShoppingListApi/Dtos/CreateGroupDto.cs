namespace SharedShoppingListApi.Dtos
{
    public class CreateGroupDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string UniqueUserId { get; set; } = null!;
    }
}
