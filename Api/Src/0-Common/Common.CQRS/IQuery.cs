using MediatR;

namespace Common.CQRS
{
    public interface IQuery<TQueryResult> : IRequest<TQueryResult>
    {
    }
}
