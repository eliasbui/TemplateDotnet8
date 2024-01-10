using MediatR;
using Template.Contract.Abstractions.Shared;

namespace Template.Contract.Abstractions.Message;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<ResultT<TResponse>>
{
}
