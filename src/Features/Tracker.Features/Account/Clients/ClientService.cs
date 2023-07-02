namespace Tracker.Features.Account.Clients
{
    public class ClientService
    {
        private readonly CollectionProvider _provider;
        private readonly IConfiguration _configuration;
        public HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        public ClientService( IConfiguration configuration, CollectionProvider provider)
        {
            _provider = provider;
            _configuration = configuration;
        }

        public async Task<ClientsDto> GetDetaultClient()
        {
            var Clients = _provider.GetCollection<ClientsDto>(CollectionNames.Clients);
           return await (await Clients.FindAsync(x => x.Name == _configuration.GetRequiredSection("Appsettings")["DefaultClient"].ToString())).FirstOrDefaultAsync();
            
        }
        public async Task<ClientsDto> GetClient()
        {
            var Clients = _provider.GetCollection<ClientsDto>(CollectionNames.Clients);
            return await (await Clients.FindAsync(x => x.Id == _httpContext.GetClientIdClaimValue())).FirstOrDefaultAsync();

        }
        public async Task CreateClient(ClientsDto request)
        {
            var Clients = _provider.GetCollection<ClientsDto>(CollectionNames.Clients);
             await Clients.InsertOneAsync(request);

        }
    }
}
