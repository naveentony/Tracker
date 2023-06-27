using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Domain.Dtos;
using Tracker.Domain.Settings;

namespace Tracker.Domain.Provider
{

    public interface ICollectionProvider
    {
        IMongoCollection<T> GetCollection<T>(string collection);
        MongoClient GetClient();
        Task<(int totalPages, List<T> readOnlyList, long count)> QueryByPage<T>(IMongoCollection<T> collection, DataFilter filter);
        Task<T> GetCollectionFristOrDefautFilter<T>(string collection, string ColumnName, string Value);
        Task<List<T>> GetCollectionListFilter<T>(string collection, string ColumnName, string Value);
    }
    public class CollectionProvider : ICollectionProvider
    {
        private readonly TrackerSettings _settings;
        public CollectionProvider(IConfiguration config)
        {
            var settings = config.GetSection("TrackerSettings").Get<TrackerSettings>();
            // var settings = config.GetRequiredSection("TrackerSettings").Get<TrackerSettings>();
            if (settings != null)
                _settings = settings;
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            var client = GetClient();
            var database = client.GetDatabase(_settings.DatabaseName);
            return database.GetCollection<T>(collectionName);
        }
        public async Task<List<T>> GetCollectionListFilter<T>(string collectionName,string ColumnName,string Value)
        {
            var client = GetClient();
            var database = client.GetDatabase(_settings.DatabaseName);
            var list = await (await database.GetCollection<T>(collectionName).FindAsync(Builders<T>.Filter.Eq(ColumnName, Value))).ToListAsync().ConfigureAwait(false);
            return list;
        }
        public async Task<T> GetCollectionFristOrDefautFilter<T>(string collectionName, string ColumnName, string Value)
        {
            var client = GetClient();
            var database = client.GetDatabase(_settings.DatabaseName);
            var    lst  = await database.GetCollection<T>(collectionName).FindAsync(Builders<T>.Filter.Eq(ColumnName, Value)).ConfigureAwait(false);
            return lst.FirstOrDefault();
        }
        public MongoClient GetClient()
        {
            return new MongoClient(_settings.ConnectionString);
        }

        public async Task<(int totalPages, List<T> readOnlyList, long count)> QueryByPage<T>(IMongoCollection<T> collection, DataFilter filter)
        {
            var countFacet = AggregateFacet.Create("count",
              PipelineDefinition<T, AggregateCountResult>.Create(new[]
              {
                PipelineStageDefinitionBuilder.Count<T>()
              }));

            var dataFacet = AggregateFacet.Create("data",
                PipelineDefinition<T, T>.Create(new[]
                {
                PipelineStageDefinitionBuilder.Sort(Builders<T>.Sort.Ascending(filter.Orderby)),
                PipelineStageDefinitionBuilder.Skip<T>((filter.PageNumber - 1) * filter.PageSize),
                PipelineStageDefinitionBuilder.Limit<T>(filter.PageSize),
                }));

            var Datafilter = Builders<T>.Filter.Empty;
            var DatafilterId = Builders<T>.Filter.Eq(filter.ColumnName, filter.ColumnValue);
            var aggregation = await collection.Aggregate()
                .Match(filter.FilterID==null? Datafilter: DatafilterId)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "count")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            var totalPages = (int)count / filter.PageSize;

            var data = aggregation.First()
                .Facets.First(x => x.Name == "data")
                .Output<T>().ToList();

            return (totalPages, data, count);
        }
    }
}
