using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Application.Shared;
using Tracker.Domain.Dtos;
using Tracker.Domain.Provider;

namespace Tracker.Application.Services
{
    //https://stackoverflow.com/questions/30685849/mongodb-c-sharp-2-0-driver-multiple-unwinds#:~:text=var%20collection%20%3D%20database.GetCollection%3CA%3E%20%28%22As%22%29%3B%20var%20result%20%3D,%28%22%24sum%22%2C%20%22%24Bs.Cs.Amount%22%29%7D%7D%29.Sort%20%28new%20BsonDocument%20%28%22total%22%2C%20-1%29%29.Limit%20%2810%29.ToListAsync%20%28%29%3B
    public class UserService
    {
        private readonly CollectionProvider _provider;
        private readonly IConfiguration _configuration;
        public HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        public UserService(IConfiguration configuration, CollectionProvider provider)
        {
            _provider = provider;
            _configuration = configuration;
        }
        //public async Task<UsersDto> CreateUser(UsersDto usersDto)
        //{
        //    if (usersDto is not null)
        //    {

        //    }
        //}

        //public async Task<UsersDto> GetDetaultClient()
        //{
        //    var users = _provider.GetCollection<UsersDto>(CollectionNames.USERS);
        //    var docs = users.Aggregate().Unwind(x => x.Roles)
        //            .Lookup("Roles", "Name", "_id", "asAccounts")
        //            .Lookup("user", "UserId", "_id", "asUsers")
        //            .Lookup("setting", "SettingId", "_id", "asSettings")
        //            .As<UsersDto>()
        //            .ToList();

        //    //users.Aggregate().Lookup("","",)
        //    //        [{
        //    //$lookup:
        //    //            {
        //    //            from: "books_selling_data",
        //    //        localField: "isbn",
        //    //        foreignField: "isbn",
        //    //        as: "copies_sold"
        //    //    }
        //    //        }])
        //  //  return (await Clients.FindAsync(x => x.Name == _configuration.GetRequiredSection("Appsettings")["DefaultClient"].ToString())).FirstOrDefault();
        //}
    }
}
