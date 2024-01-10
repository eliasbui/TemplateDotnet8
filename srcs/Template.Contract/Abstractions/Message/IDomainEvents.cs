using MediatR;

namespace Template.Contract.Abstractions.Message;

public interface IDomainEvents : INotification
{
    public Guid IdEvent { get; init; }
}
