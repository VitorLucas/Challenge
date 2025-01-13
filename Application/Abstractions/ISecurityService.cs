using Challenge.Domain;

namespace Challenge.Application.Abstractions;

public interface ISecurityService
{
    Task<IEnumerable<IsinModel>> GetIsinPriceAsync(List<string> isinId);
}
