﻿namespace miniTodo.Api.Services.JwtGenerator.Models;

public class RefreshTokenModel
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
