namespace Expenses.Models;

public class UserModel
{
    public Guid UserId { get; init; } 
    public string Username { get; private set; }
    public string Password { get; private set; }
    public string[] Roles { get; private set; }
    public DateTime DateCreated { get; private set; }

    public UserModel() {}
    
    public UserModel(string username, string password, string[] roles)
    {
        UserId = Guid.NewGuid();
        Username = username;
        Password = password;
        Roles = roles;
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

    public void ChangeRoles(string[] newRoles)
    {
        Roles = newRoles;
    }
}