using Challenge.Domain;
using Challenge.Infrastructure.Abstractions;

namespace Challenge.Api.Infrastructure.Repositories;

public class SecurityRepository : ISecurityRepository
{
    public Task<IsinModel?> InsertIsinAsync(IsinModel model)
    {
        throw new NotImplementedException();
    }
}
