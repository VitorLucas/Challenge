using Challenge.Application;
using Challenge.Infrastructure.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;

namespace SecurityServiceTest
{
    public class SecurityServiceTest
    {
        private readonly SecurityService securityService;
        public SecurityServiceTest()
        {

            securityService = new SecurityService(new Mock<ILogger<SecurityService>>().Object,
                                                    new Mock<IHttpClientFactory>().Object,
                                                   new Mock<ISecurityRepository>().Object);
        }

        [Fact]
        public async void SecurityService_GetIsinPrice_InputListEmpty()
        {
            //Arrange/act
            var result = await securityService.GetIsinPricesAsync(new List<string>());

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async void SecurityService_GetIsinPrice_InputLisNull()
        {
            //Arrange/act
            var result = await securityService.GetIsinPricesAsync(null);

            //Assert
            Assert.Empty(result);
        }
    }
}