using System.Threading;
using System.Threading.Tasks;

namespace MongoDDD.Core
{
    public interface IRepository<TAggregate, TAggregateState> 
        where TAggregate : Aggregate<TAggregateState>, new()
    {
        Task Add(TAggregate aggregate, CancellationToken token);

        Task<TAggregate> Get(string id, CancellationToken token);

        Task Save(TAggregate aggregate, CancellationToken token);

        Task Remove(string id, CancellationToken token);
    }
}
