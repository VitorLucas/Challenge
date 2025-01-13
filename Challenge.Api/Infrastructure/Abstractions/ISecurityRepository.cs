using Challenge.Domain;

namespace Challenge.Infrastructure.Abstractions;

public interface ISecurityRepository
{
    Task<IsinModel?> InsertIsinAsync(IsinModel model);
}
