using MediatR;

namespace Cart.Api.CQRS
{
    public interface ICommand : ICommand<Unit> { }
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
