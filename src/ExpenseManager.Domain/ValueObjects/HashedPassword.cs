namespace ExpenseManager.Domain.ValueObjects;

public record HashedPassword
{
    public string Hash { get; init; }
    public string Salt { get; init; }

    public HashedPassword(string hash, string salt)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Hash cannot be empty", nameof(hash));

        if (string.IsNullOrWhiteSpace(salt))
            throw new ArgumentException("Salt cannot be empty", nameof(salt));

        Hash = hash;
        Salt = salt;
    }
}