// ./Domain/User.cs

namespace DistributedLibrary.Main.Domain
{
    /// <summary>
    /// This is the the User entity
    /// </summary>
    internal sealed class User
    {
        public Guid Id { get; private set; } = default!; // We will be assigning this in constructor for better testing
        public string Username { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string? PasswordHash { get; private set; } = null;
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; private set; } = null;
        public Guid Version { get; private set; } = Guid.NewGuid(); // Concurrency token

        // EF
        private User() { }

        // CONSTRUCTOR
        public User(Guid id, string username, string email)
        {
            Id = id;
            Username = username;
            Email = email;
        }

        /// <summary>
        /// We set the password hash after creating the user
        /// </summary>
        /// <param name="passwordHash"></param>
        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }

        /// <summary>
        /// Changes the username
        /// </summary>
        /// <param name="username"></param>
        public void ModifyUsername(string username)
        {
            if (!string.IsNullOrWhiteSpace(username) && username != Username)
            {
                Username = username;
                UpdatedAt = DateTimeOffset.UtcNow;
                Version = Guid.NewGuid();
            }
        }

        /// <summary>
        /// Changes the email
        /// </summary>
        /// <param name="email"></param>
        public void ModifyEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email) && email != Email)
            {
                Email = email;
                UpdatedAt = DateTimeOffset.UtcNow;
                Version = Guid.NewGuid();
            }
        }
    }
}
