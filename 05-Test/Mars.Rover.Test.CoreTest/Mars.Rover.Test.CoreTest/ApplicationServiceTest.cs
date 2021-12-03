using FluentAssertions;
using Mars.Rover.Application.Contract.Services;
using Mars.Rover.Application.Service;
using Mars.Rover.Core.Helper;
using Mars.Rover.Domain.Aggregate.Navigate;
using Mars.Rover.Domain.Contract.DTO.Navigate;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mars.Rover.Test.CoreTest
{
    public class ApplicationServiceTest
    {
        private IApplicationService applicationService { get; set; }

        public ApplicationServiceTest()
        {
            var serviceProvider = new ServiceCollection()
               .AddSingleton<IApplicationService, ApplicationService>()
               .AddSingleton<INavigateService, NavigateService>()
               .BuildServiceProvider();
            this.applicationService = serviceProvider.GetService<IApplicationService>();
        }

        [Fact]
        public async Task NavigateMarsRoverShouldBeExpectedResult()
        {   // Arrange
            ResponseListDTO<NavigateMarsRoverResponseDTO> responseList = new ResponseListDTO<NavigateMarsRoverResponseDTO>();
            NavigateMarsRoverRequestDTO requestDTO = new NavigateMarsRoverRequestDTO();
            string[] parameters = new string[5] { "5 5", "1 2 N", "LMLMLMLMM", "3 3 E", "MMRMMRMRRM" };
            requestDTO.Parameters = parameters.ToList();

            // Act
            responseList = await this.applicationService.NavigateMarsRover(requestDTO);

            // Assert
            responseList.Should().NotBeNull();
            responseList.Data.ToList().FirstOrDefault().ResponseString.Should().Be("1 3 N");
            responseList.Data.ToList().LastOrDefault().ResponseString.Should().Be("5 1 E");
            responseList.RC.Should().Be("RC0000");
        }

        [Fact]
        public async Task NavigateMarsRoverShouldBeSingleResult()
        {   // Arrange
            ResponseListDTO<NavigateMarsRoverResponseDTO> responseList = new ResponseListDTO<NavigateMarsRoverResponseDTO>();
            NavigateMarsRoverRequestDTO requestDTO = new NavigateMarsRoverRequestDTO();
            string[] parameters = new string[4] { "5 5", "1 2 N", "LMLMLMLMM", "3 3 E" };
            requestDTO.Parameters = parameters.ToList();

            // Act
            responseList = await this.applicationService.NavigateMarsRover(requestDTO);

            // Assert
            responseList.Should().NotBeNull();
            responseList.Data.ToList().FirstOrDefault().ResponseString.Should().Be("1 3 N");
        }

        [Fact]
        public async Task NavigateMarsRoverShouldBeSingleResult2()
        {    // Arrange
            ResponseListDTO<NavigateMarsRoverResponseDTO> responseList = new ResponseListDTO<NavigateMarsRoverResponseDTO>();
            NavigateMarsRoverRequestDTO requestDTO = new NavigateMarsRoverRequestDTO();
            string[] parameters = new string[3] { "5 5", "1 2 N", "LMLMLMLMM" };
            requestDTO.Parameters = parameters.ToList();

            // Act
            responseList = await this.applicationService.NavigateMarsRover(requestDTO);

            // Assert
            responseList.Should().NotBeNull();
            responseList.Data.ToList().FirstOrDefault().ResponseString.Should().Be("1 3 N");
        }

        [Fact]
        public async Task NavigateMarsRoverShouldBeErrorMessage()
        {   // Arrange
            NavigateMarsRoverRequestDTO requestDTO = new NavigateMarsRoverRequestDTO();
            string[] parameters = new string[1] { "5 5" };
            requestDTO.Parameters = parameters.ToList();

            // Act
            var exception = Assert.ThrowsAsync<System.Exception>(() => this.applicationService.NavigateMarsRover(requestDTO));

            // Assert
            Assert.Equal("Request is not valid", exception.Result.Message);
        }
    }
}
