namespace Challenge.Application.Abstractions;

public interface ISecurityProviderService
{
    Task<decimal> FindIsinPriceById(string IsinId);
}
