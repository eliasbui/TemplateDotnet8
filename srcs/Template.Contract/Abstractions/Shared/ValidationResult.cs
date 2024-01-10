namespace Template.Contract.Abstractions.Shared;

public class ValidationResult : Result, IValidationResult
{
    protected internal ValidationResult(Error[] errors) : base(false, IValidationResult.ValidationErrors)
        => Errors = errors;

    public new Error[] Errors { get; }

    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}
