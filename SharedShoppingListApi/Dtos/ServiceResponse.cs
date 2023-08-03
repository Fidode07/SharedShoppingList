namespace SharedShoppingListApi.Dtos
{
    public class ServiceResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; } = false;
    }
    public class ServiceResponse<T> : ServiceResponse
    {
        public T? Data { get; set; }
    }
}
