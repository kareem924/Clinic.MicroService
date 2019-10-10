using System;
using System.Collections.Generic;
using System.Text;

namespace Common.CQRS
{
    public interface IQueryHandler<TQuery, TQueryResult> : IRequestHandler<TQuery, TQueryResult>
        where TQuery : IQuery<TQueryResult>
    {
        Task<TQueryResult> Handle(TQuery Query, TQueryResult QueryResult);
    }
}
