﻿namespace UserService.Application.Models;

public class AuthModel
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}