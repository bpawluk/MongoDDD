using System.Threading;
using System.Threading.Tasks;

namespace MongoDDD.Core.Queries
{
    public interface IWriteQuery<TQueryData>
    {
        Task Write(TQueryData queryData, CancellationToken token);
    }

    public interface IWriteQuery<TQueryData, TQueryResult>
    {
        Task<TQueryResult> Write(TQueryData queryData, CancellationToken token);
    }
}
