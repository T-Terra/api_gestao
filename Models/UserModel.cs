﻿using System.ComponentModel.DataAnnotations;

namespace Expenses.Models;

public class UserModel
{
    [Key]
    public Guid UserId { get; init; } 
    public string Username { get; private set; }
    public string Password { get; private set; }
    [Required]
    public string Email { get; private set; }
    public string[] Roles { get; private set; }
    public DateTime DateCreated { get; private set; }

    public UserModel() {}
    
    public UserModel(string username, string password, string email) 
    {
        UserId = Guid.NewGuid();
        Username = username;
        Password = password;
        Email = email;
        Roles = ["viewer"];
        DateCreated = DateTime.UtcNow;
    }

    public void ChangeUsername(string newUsername)
    {
        Username = newUsername;
    }
    
    public void ChangePassword(string newPassword)
    {
        Password = newPassword;
    }

    public void ChangeEmail(string newEmail)
    {
        Email = newEmail;
    }

    public void ChangeRoles(string[] newRoles)
    {
        Roles = newRoles;
    }
}