using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Domain.Settings;

namespace Tracker.Domain
{
    //Models
    public record Student
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = new Guid().ToString();
        [Required, MinLength(2)]
        [Column(Order = 2)]
        [StringLength(100)]
        public string Name { get; set; }

        [Column(Order = 3)]
        public string Address { get; set; }

        [Required, EmailAddress]
        [Column(Order = 4)]
        public string Email { get; set; }

        [Column(Order = 5)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [Column(Order = 6)]
        public bool? Active { get; set; }
    }

    public interface IDataContext
    {
        IMongoCollection<Student> Student { get; }
    }
    public class DataContext : IDataContext
    {

        public DataContext(IConfiguration configuration)
        {
            var settings = configuration.GetSection("TrackerSettings");
            
            var client = new MongoClient(settings["ConnectionString"]);
            var database = client.GetDatabase(settings["DatabaseName"]);
            Student = database.GetCollection<Student>(settings["CollectionName"]);
            // CatalogContextSeed.SeedData(Student);

        }
        public IMongoCollection<Student> Student { get; }
    }
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Student> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetPreconfiguredProducts());
            }
        }

        private static IEnumerable<Student> GetPreconfiguredProducts()
        {
            return new List<Student>()
            {
                new Student()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "IPhone X",
                    Address  = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Email="Naveen@gmail.com",
                      Active=true,
                      DateOfBirth=DateTime.Now
                }
            };
        }
    }
}
