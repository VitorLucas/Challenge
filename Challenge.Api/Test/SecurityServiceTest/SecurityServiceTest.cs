using Challenge.Application.Abstractions;
using Challenge.Infrastructure.Abstractions;
using Challenge.Application;
using Moq;

namespace SecurityServiceTest
{
    public class SecurityServiceTest
    {
        private readonly SecurityService securityService;
        public SecurityServiceTest()
        {

            securityService = new SecurityService(new Mock<ISecurityProviderService>().Object,
                                                   new Mock<ISecurityRepository>().Object);
        }

        [Fact]
        public async void SecurityService_GetIsinPrice_InputListEmpty()
        {
            //Arrange/act
            var result = await securityService.GetIsinPriceAsync(new List<string>());

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async void SecurityService_GetIsinPrice_InputLisNull()
        {
            //Arrange/act
            var result = await securityService.GetIsinPriceAsync(null);

            //Assert
            Assert.Empty(result);
        }
    }
}