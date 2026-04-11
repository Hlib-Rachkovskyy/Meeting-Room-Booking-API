namespace Meeting_Room_Booking_API.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public int RefreshTokenVersion { get; private set; } = 0;
    public DateTime CreatedAt { get; private set; }
    
    // RBAC & Brute Force Mitigation
    public string Role { get; private set; } = "User";
    public int FailedLoginAttempts { get; private set; } = 0;
    public DateTime? LockoutEnd { get; private set; }

    // EF Core parameterless constructor
    private User() { }

    public User(string email, string passwordHash, string fullName)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.", nameof(fullName));

        Id = Guid.NewGuid();
        Email = email.ToLowerInvariant().Trim();
        PasswordHash = passwordHash;
        FullName = fullName.Trim();
        CreatedAt = DateTime.UtcNow;
        RefreshTokenVersion = 0;
    }

    public void IncrementRefreshTokenVersion()
    {
        RefreshTokenVersion++;
    }

    public void RecordFailedLogin()
    {
        FailedLoginAttempts++;
        if (FailedLoginAttempts >= 5)
        {
            LockoutEnd = DateTime.UtcNow.AddMinutes(15);
        }
    }

    public void ResetFailedLogins()
    {
        FailedLoginAttempts = 0;
        LockoutEnd = null;
    }

    public bool IsLockedOut()
    {
        return LockoutEnd.HasValue && LockoutEnd.Value > DateTime.UtcNow;
    }

    public void PromoteToAdmin()
    {
        Role = "Admin";
    }
}
