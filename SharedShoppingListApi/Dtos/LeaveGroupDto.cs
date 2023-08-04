namespace SharedShoppingListApi.Dtos
{
    public class LeaveGroupDto
    {
        public int GroupId { get; set; }
        public string UniqueUserId { get; set; } = null!;
    }
}
