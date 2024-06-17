using System.Threading;
using System.Threading.Tasks;

namespace MongoDDD.Core
{
    public interface IRepository<TAggregateRoot, TAggregateState> 
        where TAggregateRoot : AggregateRoot<TAggregateState>, new()
    {
        Task Add(TAggregateRoot aggregate, CancellationToken token);

        Task<TAggregateRoot> Get(string id, CancellationToken token);

        Task Save(TAggregateRoot aggregate, CancellationToken token);

        Task Remove(string id, CancellationToken token);
    }
}
