using Challenge.Application.Abstractions;
using Challenge.Domain;
using Challenge.Infrastructure.Abstractions;
using Challenge.Application.Abstractions;
using System.ComponentModel.Design;
using System.Net;
using Challenge.Api.Domain.Dtos;
using Challenge.Api.Application;

namespace Challenge.Application;

public class SecurityService : ServiceBase, ISecurityService
{
    private readonly int ISIN_MAX_LENGTH = 12;

    private readonly ILogger<SecurityService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISecurityRepository _securityRepository;

    public SecurityService(ILogger<SecurityService> logger,
                            IHttpClientFactory httpClientFactory,
                           ISecurityRepository securityRepository)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _securityRepository = securityRepository;
    }

    public async Task<IEnumerable<IsinModel>> GetIsinPricesAsync(List<string> isinIds)
    {
        if (isinIds == null || !isinIds.Any())
        {
            _logger.LogError($"ISIN ID list is empty.");
            return Enumerable.Empty<IsinModel>();
        }

        List<IsinModel> Isinslist = new List<IsinModel>();
        foreach (string id in isinIds)
        {
            if (id.Length > ISIN_MAX_LENGTH)
            {
                _logger.LogError($"Incorrect ISIN ID:{id}");
                continue;
            }
            var priceFound = await FindIsinPriceByIdAsync(id);
            if (priceFound is null)
            {
                _logger.LogError($"Price not found for ISIN ID:{id}");
                continue;
            }

            IsinModel newModel = new IsinModel(Guid.NewGuid(), id, (decimal)priceFound.Price);

            var modelInserted = await securityRepository.InsertIsinAsync(newModel);
            if (modelInserted is null)
            {
                _logger.LogError($"Isin could not be saved  ISIN ID:{id}");
                continue;
            }

            Isinslist.Add(modelInserted);
        }

        return Isinslist.AsEnumerable();
    }

    private async Task<IsinDto> FindIsinPriceByIdAsync(string id) 
    {
        if (!string.IsNullOrEmpty(id))
        {
            var uriSecurityServices = $"securityprice/{id}";
            var isinResponse = await _httpClientFactory.CreateClient(nameof(ISecurityService)).GetAsync(uriSecurityServices);

            if (isinResponse.StatusCode.Equals(HttpStatusCode.OK))
            {
                var isinFound = await DeserializarObjetoResponse<IsinDto>(isinResponse);

                return isinFound ?? default;
            }

            return default;
        }
        return default;
    }
}
