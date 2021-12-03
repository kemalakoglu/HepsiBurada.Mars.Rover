using FluentAssertions;
using Mars.Rover.Domain.Aggregate.Navigate;
using Mars.Rover.Domain.Contract.DTO.Navigate;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mars.Rover.Test.CoreTest
{
    public class DomainServiceTest
    {
        private INavigateService navigateService { get; set; }

        public DomainServiceTest()
        {
            var serviceProvider = new ServiceCollection()
               .AddSingleton<INavigateService, NavigateService>()
               .BuildServiceProvider();
            this.navigateService = serviceProvider.GetService<INavigateService>();
        }

        [Fact]
        public async Task NavigateMarsRoverShouldBeExpectedResult()
        {   // Arrange
            List<NavigateMarsRoverResponseDTO> responseList = new List<NavigateMarsRoverResponseDTO>();
            NavigateMarsRoverRequestDTO requestDTO = new NavigateMarsRoverRequestDTO();
            string[] parameters = new string[5] { "5 5", "1 2 N", "LMLMLMLMM", "3 3 E", "MMRMMRMRRM" };
            requestDTO.Parameters = parameters.ToList();

            // Act
            responseList = await this.navigateService.NavigateMarsRover(requestDTO);

            // Assert
            responseList.Should().NotBeNull();
            responseList.Should().NotBeEmpty();
            responseList[0].ResponseString.Should().Be("1 3 N");
            responseList[1].ResponseString.Should().Be("5 1 E");
        }

        [Fact]
        public async Task NavigateMarsRoverShouldBeSingleResult()
        {   // Arrange
            List<NavigateMarsRoverResponseDTO> responseList = new List<NavigateMarsRoverResponseDTO>();
            NavigateMarsRoverRequestDTO requestDTO = new NavigateMarsRoverRequestDTO();
            string[] parameters = new string[4] { "5 5", "1 2 N", "LMLMLMLMM", "3 3 E"};
            requestDTO.Parameters = parameters.ToList();

            // Act
            responseList = await this.navigateService.NavigateMarsRover(requestDTO);

            // Assert
            responseList.Should().NotBeNull();
            responseList.Should().NotBeEmpty();
            responseList[0].ResponseString.Should().Be("1 3 N");
        }

        [Fact]
        public async Task NavigateMarsRoverShouldBeSingleResult2()
        {   // Arrange
            List<NavigateMarsRoverResponseDTO> responseList = new List<NavigateMarsRoverResponseDTO>();
            NavigateMarsRoverRequestDTO requestDTO = new NavigateMarsRoverRequestDTO();
            string[] parameters = new string[3] { "5 5", "1 2 N", "LMLMLMLMM"};
            requestDTO.Parameters = parameters.ToList();

            // Act
            responseList = await this.navigateService.NavigateMarsRover(requestDTO);

            // Assert
            responseList.Should().NotBeNull();
            responseList.Should().NotBeEmpty();
            responseList[0].ResponseString.Should().Be("1 3 N");
        }

        [Fact]
        public async Task NavigateMarsRoverShouldBeErrorMessage()
        {   // Arrange
            NavigateMarsRoverRequestDTO requestDTO = new NavigateMarsRoverRequestDTO();
            string[] parameters = new string[1] { "5 5" };
            requestDTO.Parameters = parameters.ToList();

            // Act
            var exception= Assert.ThrowsAsync<System.Exception>(() => this.navigateService.NavigateMarsRover(requestDTO));
            // Assert
     
            Assert.Equal("Request is not valid", exception.Result.Message);
        }
    }
}
