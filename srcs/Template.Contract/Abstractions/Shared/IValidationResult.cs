namespace Template.Contract.Abstractions.Shared;

public interface IValidationResult
{
    public static readonly Error ValidationErrors = new("Validation errors occurred.", "Validation errors occurred.");
    Error[] Errors { get; }
}
