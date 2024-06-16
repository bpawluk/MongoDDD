using System.Threading;
using System.Threading.Tasks;

namespace MongoDDD.Core.Queries
{
    public interface IReadQuery<TQueryResult>
    {
        Task<TQueryResult> Read(CancellationToken token);
    }

    public interface IReadQuery<TQueryData, TQueryResult>
    {
        Task<TQueryResult> Read(TQueryData queryData, CancellationToken token);
    }
}
