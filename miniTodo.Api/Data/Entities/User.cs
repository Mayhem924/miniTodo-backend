﻿namespace miniTodo.Api.Data.Entities;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}
