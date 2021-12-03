using Mars.Rover.Domain.Contract.DTO.Navigate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Rover.Domain.Aggregate.Navigate
{
    public interface INavigateService
    {
        /// <summary>
        /// Navigates the mars rover.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<List<NavigateMarsRoverResponseDTO>> NavigateMarsRover(NavigateMarsRoverRequestDTO request);
    }
}
