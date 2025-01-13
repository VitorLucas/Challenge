using Challenge.Domain;

namespace Challenge.Application.Abstractions;

public interface ISecurityService
{
    Task<IEnumerable<IsinModel>> GetIsinPricesAsync(List<string> isinId);
}
