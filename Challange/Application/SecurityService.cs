using Challenge.Application.Abstractions;
using Challenge.Domain;
using Challenge.Infrastructure.Abstractions;
using Challenge.Application.Abstractions;

namespace Challenge.Application;

public class SecurityService : ISecurityService
{
    private readonly int ISIN_MAX_LENGTH = 12;
    private readonly ISecurityProviderService securityProviderService;
    private readonly ISecurityRepository securityRepository;

    public SecurityService(ISecurityProviderService securityProviderService,
                           ISecurityRepository securityRepository)
    {
        this.securityProviderService = securityProviderService;
        this.securityRepository = securityRepository;
    }

    public async Task<IEnumerable<IsinModel>> GetIsinPriceAsync(List<string> isinIds)
    {
        if (isinIds == null || !isinIds.Any())
        {
            Console.WriteLine($"ISIN list is empty.");
            return Enumerable.Empty<IsinModel>();
        }

        List<IsinModel> Isinslist = new List<IsinModel>();
        foreach (string id in isinIds)
        {
            if (id.Length > ISIN_MAX_LENGTH)
            {
                Console.WriteLine($"Incorrect ISIN ID:{id}");
                continue;
            }
            var priceFound = await FindIsinPriceById(id);
            if (priceFound is null)
            {
                Console.WriteLine($"Price not found for ISIN ID:{id}");
                continue;
            }

            IsinModel newModel = new IsinModel(Guid.NewGuid(), id, (decimal)priceFound);

            var modelInserted = await securityRepository.InsertIsinAsync(newModel);
            if (modelInserted is null)
            {
                Console.WriteLine($"Isin could not be saved  ISIN ID:{id}");
                continue;
            }

            Isinslist.Add(modelInserted);
        }

        return Isinslist.AsEnumerable();
    }

    private async Task<decimal?> FindIsinPriceById(string isinId) => await securityProviderService.FindIsinPriceById(isinId);
}
