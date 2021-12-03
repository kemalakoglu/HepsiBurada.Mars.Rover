using Mars.Rover.Core.Helper;
using Mars.Rover.Domain.Contract.DTO.Navigate;
using System.Threading.Tasks;


namespace Mars.Rover.Application.Contract.Services
{
    public interface IApplicationService
    {
        Task<ResponseListDTO<NavigateMarsRoverResponseDTO>> NavigateMarsRover(NavigateMarsRoverRequestDTO request);
    }
}
