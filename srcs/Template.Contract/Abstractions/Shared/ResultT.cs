namespace Template.Contract.Abstractions.Shared;

public class ResultT<TValue> : Result
{
    private readonly TValue? _value;

    protected internal ResultT(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
        => _value = value;

    public TValue Value =>
        IsSuccess ? _value! : throw new InvalidOperationException("Failed result doesn't contain value.");

    public static implicit operator ResultT<TValue>(TValue? value) => (ResultT<TValue>)Create(value);
}
