using Amazon.Runtime.Internal;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.Collections;
using System.Transactions;
using Tracker.Application.Shared;
using Tracker.Domain.Dtos;
using TransactionOptions = MongoDB.Driver.TransactionOptions;

class Program
{
    static async Task Main(string[] args)
    {
        // For a replica set, include the replica set name and a seedlist of the members in the URI string; e.g.
        // string uri = "mongodb://mongodb0.example.com:27017,mongodb1.example.com:27017/?replicaSet=myRepl";
        // For a sharded cluster, connect to the mongos instances; e.g.
        string uri = "mongodb+srv://Tracker:Tracker123$@tracker.gdpaihu.mongodb.net/?retryWrites=true&w=majority";
        var client = new MongoClient(uri);
        // Prereq: Create collections.
        var database1 = client.GetDatabase("mydb1");
        var collection1 = database1.GetCollection<BsonDocument>("foo").WithWriteConcern(WriteConcern.WMajority);
        collection1.InsertOne(new BsonDocument("abc", 0));
        var database2 = client.GetDatabase("mydb2");
        var collection2 = database2.GetCollection<BsonDocument>("bar").WithWriteConcern(WriteConcern.WMajority);
        collection2.InsertOne(new BsonDocument("xyz", 0));
        // Step 1: Start a client session.
        using (var session = client.StartSession())
        {
            // Step 2: Optional. Define options to use for the transaction.
            var transactionOptions = new TransactionOptions(
                writeConcern: WriteConcern.WMajority);
            // Step 3: Define the sequence of operations to perform inside the transactions
            var cancellationToken = CancellationToken.None; // normally a real token would be used
            var result = session.WithTransaction(
                (s, ct) =>
                {
                    collection1.InsertOne(s, new BsonDocument("abc", 1), cancellationToken: ct);
                    collection2.InsertOne(s, new BsonDocument("xyz", 999), cancellationToken: ct);
                    return "Inserted into collections in different databases";
                },
                transactionOptions,
                cancellationToken);
        }
        //var client = new MongoClient();
        //var database = client.GetDatabase("Tracker");
        //var userDto = GetCollection<UsersDto>(CollectionNames.USERS);
        //var RolesDto = GetCollection<MongoRoleDto>(CollectionNames.ROLES);

        //using (var session = await client.StartSessionAsync())
        //{
        //    // Begin transaction
        //    session.StartTransaction();
        //    try
        //    {

        //        //var Deviceerequest = DeviceRegister.TodeviceVehiclesDto(request);
        //        //Deviceerequest.CreatedDate = DateTime.Now;
        //        //await CollectionName.InsertOneAsync(Deviceerequest).ConfigureAwait(false);
        //        //var assignid = await _assignVehicleService.AssignVehile(Deviceerequest.Id);
        //        //await _alertsService.AddAlert(assignid, AlertNameType.WhatsApp.ToString(), AlertType.PowerVoid.ToString(), Deviceerequest.Id, 5, 5);
        //        ////Made it here without error? Let's commit the transaction
        //        await session.CommitTransactionAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //       // _result.AddError(ErrorCode.DatabaseOperationException, ex.Message);
        //        await session.AbortTransactionAsync();
        //    }

    //}
    }
    public static IMongoCollection<T> GetCollection<T>(string collection)
    {
        var client = new MongoClient();
        var database = client.GetDatabase("Tracker");
        return database.GetCollection<T>(collection);
    }
    private static async Task<(int totalPages, IReadOnlyList<T> readOnlyList)> QueryByPage<T>(int page, int pageSize, IMongoCollection<T> collection)
    {
        var countFacet = AggregateFacet.Create("count",
            PipelineDefinition<T, AggregateCountResult>.Create(new[]
            {
                PipelineStageDefinitionBuilder.Count<T>()
            }));

        var dataFacet = AggregateFacet.Create("data",
            PipelineDefinition<T, T>.Create(new[]
            {
                PipelineStageDefinitionBuilder.Sort(Builders<T>.Sort.Ascending("Vehicle")),
                PipelineStageDefinitionBuilder.Skip<T>((page - 1) * pageSize),
                PipelineStageDefinitionBuilder.Limit<T>(pageSize),
            }));

        var filter = Builders<T>.Filter.Empty;
        var aggregation = await collection.Aggregate()
            .Match(filter)
            .Facet(countFacet, dataFacet)
            .ToListAsync();

        var count = aggregation.First()
            .Facets.First(x => x.Name == "count")
            .Output<AggregateCountResult>()
            ?.FirstOrDefault()
            ?.Count ?? 0;

        var totalPages = (int)count / pageSize;

        var data = aggregation.First()
            .Facets.First(x => x.Name == "data")
            .Output<T>();

        return (totalPages, data);
    }

    private static int i = 1;

    private static void WriteResults(int page, IReadOnlyList<Person> readOnlyList)
    {
        Console.WriteLine($"Page: {page}");

        foreach (var person in readOnlyList)
        {
            Console.WriteLine($"{i}: {person.Amount} {person.Vehicle}");
            i++;
        }
    }
    // For a replica set, include the replica set name and a seedlist of the members in the URI string; e.g.
    // string uri = "mongodb://mongodb0.example.com:27017,mongodb1.example.com:27017/?replicaSet=myRepl";
    // For a sharded cluster, connect to the mongos instances; e.g.
    // string uri = "mongodb://mongos0.example.com:27017,mongos1.example.com:27017/";
//    var client = new MongoClient(connectionString);

//    // Prereq: Create collections.
//    var database1 = client.GetDatabase("mydb1");
//    var collection1 = database1.GetCollection<BsonDocument>("foo").WithWriteConcern(WriteConcern.WMajority);
//    collection1.InsertOne(new BsonDocument("abc", 0));

//var database2 = client.GetDatabase("mydb2");
//    var collection2 = database2.GetCollection<BsonDocument>("bar").WithWriteConcern(WriteConcern.WMajority);
//    collection2.InsertOne(new BsonDocument("xyz", 0));

//// Step 1: Start a client session.
//using (var session = client.StartSession())
//{
//    // Step 2: Optional. Define options to use for the transaction.
//    var transactionOptions = new TransactionOptions(
//        writeConcern: WriteConcern.WMajority);

//    // Step 3: Define the sequence of operations to perform inside the transactions
//    var cancellationToken = CancellationToken.None; // normally a real token would be used
//    result = session.WithTransaction(
//        (s, ct) =>
//        {
//            collection1.InsertOne(s, new BsonDocument("abc", 1), cancellationToken: ct);
//            collection2.InsertOne(s, new BsonDocument("xyz", 999), cancellationToken: ct);
//            return "Inserted into collections in different databases";
//        },
//        transactionOptions,
//        cancellationToken);
//}
   
}

public class Person
{
    public ObjectId Id { get; set; } //= new Guid().ToString();
    public string Vehicle { get; set; }
    public double Amount { get; set; }
    public string Status { get; set; }

}