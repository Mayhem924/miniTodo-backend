namespace miniTodo.Api.Data.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }

    public User User { get; set; }
    public Guid UserId { get; set; }
}
