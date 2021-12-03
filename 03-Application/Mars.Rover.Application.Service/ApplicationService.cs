using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mars.Rover.Application.Contract.Services;
using Mars.Rover.Core.Helper;
using Mars.Rover.Domain.Aggregate.Navigate;
using Mars.Rover.Domain.Contract.DTO.Navigate;

namespace Mars.Rover.Application.Service
{
    public class ApplicationService : IApplicationService
    {
        private INavigateService navigateService { get; set; }

        public ApplicationService(INavigateService navigateService)
        {
            this.navigateService = navigateService;
        }

        /// <summary>
        /// Navigates the mars rover.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<ResponseListDTO<NavigateMarsRoverResponseDTO>> NavigateMarsRover(NavigateMarsRoverRequestDTO request)
        {
            List<NavigateMarsRoverResponseDTO> responseList = new List<NavigateMarsRoverResponseDTO>();

            responseList = await this.navigateService.NavigateMarsRover(request);

            return await CreateAsyncResponse<NavigateMarsRoverResponseDTO>.Return(responseList, "NavigateMarsRover");
        }
    }
}