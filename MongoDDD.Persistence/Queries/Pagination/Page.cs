using System.Collections.Generic;

namespace MongoDDD.Persistence.Queries.Pagination
{
    public class Page<TData, TExternalData>
    {
        public int Number { get; }

        public int OutOf { get; }

        public IEnumerable<DatabaseDocument<TData, TExternalData>> Documents { get; }

        public Page(int number, int outOf, IEnumerable<DatabaseDocument<TData, TExternalData>> documents)
        {
            Number = number;
            OutOf = outOf;
            Documents = documents;
        }
    }
}
