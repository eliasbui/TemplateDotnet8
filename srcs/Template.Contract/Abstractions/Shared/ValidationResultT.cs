namespace Template.Contract.Abstractions.Shared;

public class ValidationResultT<T> : ResultT<T>, IValidationResult
{
    protected internal ValidationResultT(Error[] errors) : base(default!, false, IValidationResult.ValidationErrors)
        => Errors = errors;

    public new Error[] Errors { get; }

    public static ValidationResultT<T> WithErrors(Error[] errors) => new(errors);
}
