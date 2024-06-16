using MongoDB.Driver;
using MongoDDD.Core.Exceptions;
using MongoDDD.Persistence.Queries.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDDD.Persistence.Queries
{
    public partial class DatabaseQuery<TData, TExternalData>
    {
        protected async Task<DatabaseDocument<TData, TExternalData>> FindOne(
            FilterDefinition<DatabaseDocument<TData, TExternalData>> filter, 
            CancellationToken token)
        {
            var cursor = await _collection.FindAsync(filter, cancellationToken: token);
            var document = await cursor.FirstOrDefaultAsync(cancellationToken: token);

            if (document is null)
            {
                throw new DoesNotExistException($"{typeof(TData).Name} that matches the requested query does not exist.");
            }

            return document;
        }

        protected async Task<IEnumerable<DatabaseDocument<TData, TExternalData>>> FindMany(
            FilterDefinition<DatabaseDocument<TData, TExternalData>> filter,
            CancellationToken token)
        {
            var cursor = await _collection.FindAsync(filter, cancellationToken: token);
            return await cursor.ToListAsync(token);
        }

        protected async Task<IEnumerable<DatabaseDocument<TData, TExternalData>>> FindMany(
            FilterDefinition<DatabaseDocument<TData, TExternalData>> filter,
            SortDefinition<DatabaseDocument<TData, TExternalData>> sort,
            CancellationToken token)
        {
            var options = new FindOptions<DatabaseDocument<TData, TExternalData>>
            {
                Sort = sort
            };
            var cursor = await _collection.FindAsync(filter, options, token);
            return await cursor.ToListAsync(token);
        }

        protected async Task<Page<TData, TExternalData>> FindMany(
            FilterDefinition<DatabaseDocument<TData, TExternalData>> filter,
            SortDefinition<DatabaseDocument<TData, TExternalData>> sort,
            PageInfo pageInfo,
            CancellationToken token)
        {
            var countFacet = AggregateFacet.Create(
                "count", 
                PipelineDefinition<DatabaseDocument<TData, TExternalData>, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<DatabaseDocument<TData, TExternalData>>()
                }));

            var dataFacet = AggregateFacet.Create(
                "data",
                PipelineDefinition<DatabaseDocument<TData, TExternalData>, DatabaseDocument<TData, TExternalData>>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Skip<DatabaseDocument<TData, TExternalData>>((pageInfo.Number - 1) * pageInfo.Size),
                    PipelineStageDefinitionBuilder.Limit<DatabaseDocument<TData, TExternalData>>(pageInfo.Size),
                }));

            var aggregation = await _collection.Aggregate()
                .Match(filter)
                .Sort(sort)
                .Facet(countFacet, dataFacet)
                .SingleAsync(cancellationToken: token);

            double count = aggregation.Facets
                .Single(x => x.Name == "count")
                .Output<AggregateCountResult>()
                .SingleOrDefault()
                ?.Count ?? 0;
            var numberOfPages = (int)Math.Ceiling(Math.Max(count, 1) / pageInfo.Size);

            var documents = aggregation.Facets
                .First(x => x.Name == "data")
                .Output<DatabaseDocument<TData, TExternalData>>()
                .ToList();

            return new Page<TData, TExternalData>(pageInfo.Number, numberOfPages, documents);
        }
    }
}
