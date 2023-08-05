namespace SharedShoppingListApi.Dtos
{
    public class UpdateShoppingListDto
    {
        public int GroupId { get; set; }
        public string UniqueUserId { get; set; } = string.Empty;
        public string NewShoppingList { get; set; } = string.Empty;
    }
}
