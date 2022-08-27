using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Repository;

public class TrailRepository : Repository<Trail>, ITrailRepository
{
    private readonly IHttpClientFactory _clientFactory;

    public TrailRepository(IHttpClientFactory clientFactory) : base(clientFactory)
    {
        _clientFactory = clientFactory;
    }
}
