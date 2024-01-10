namespace Template.Contract.Abstractions.Shared;

public sealed class Error(string code, string message) : IEquatable<Error> // Equatable is used for testing purposes.
{
    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Message);
    }

    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error Unknown = new("Unknown", "An unknown error has occurred.");
    public static readonly Error NullValue = new("Error.NullValue", "The value cannot be null.");

    private string Code { get; } = code;
    private string Message { get; } = message;

    // This is used for testing purposes.
    public static implicit operator string (Error error) => error.Code;

    public static bool operator ==(Error? a, Error? b)
    {
        if(a is null && b is null)
        {
            return true; // This is the implementation of IEquatable<Error>.
        }
        if(a is null || b is null)
        {
            return false; // This is the implementation of IEquatable<Error>.
        }

        return a.Equals(b); // This is the implementation of IEquatable<Error>.
    }

    public static bool operator != (Error? a, Error? b) => !(a == b);
    public bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }
        return Code == other.Code && Message == other.Message;
    }
    public override bool Equals (object? obj) => obj is Error other && Equals(other);
}
