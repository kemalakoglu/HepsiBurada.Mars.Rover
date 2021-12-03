using Mars.Rover.Domain.Contract.DTO.Navigate;
using Mars.Rover.Presentation.API.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Rover.Domain.Aggregate.Navigate
{
    public class NavigateService : INavigateService
    {
        /// <summary>
        /// Navigates the mars rover.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<List<NavigateMarsRoverResponseDTO>> NavigateMarsRover(NavigateMarsRoverRequestDTO request)
        {
            IsRequestIsValid(request);
            List<NavigateMarsRoverResponseDTO> responseList = new List<NavigateMarsRoverResponseDTO>();

            Plataeu plataeu = CheckPlataeu(request.Parameters[0]);

            NavigateMarsRoverResponseDTO responseItem = new NavigateMarsRoverResponseDTO();

            for (int i = 1; i < request.Parameters.Count - 1; i = i + 2)
            {
                responseItem = GetResponseItem(request.Parameters[i], request.Parameters[i + 1], plataeu.Size);
                responseList.Add(responseItem);
            }

            return responseList;
        }

        /// <summary>
        /// Determines whether [is request is valid] [the specified request].
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="Exception">
        /// Request is null
        /// or
        /// Request is not valid
        /// </exception>
        private void IsRequestIsValid(NavigateMarsRoverRequestDTO request)
        {
            if (request==null)
            {
                throw new Exception("Request is null");
            }

            if (request.Parameters == null || request.Parameters.Count<3)
            {
                throw new Exception("Request is not valid");
            }
        }

        /// <summary>
        /// Gets the response item.
        /// </summary>
        /// <param name="rover">The rover.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="surface">The surface.</param>
        /// <returns></returns>
        private NavigateMarsRoverResponseDTO GetResponseItem(string rover, string movements, Surface surface)
        {
            NavigateMarsRoverResponseDTO response = new NavigateMarsRoverResponseDTO();
            MarsRover marsRover = ParseMarsRoverQuery(rover, surface);
            CheckIfLocationIsValid(marsRover.X, marsRover.Y, surface);
            MoveRover(movements, ref marsRover);
            response.ResponseString = marsRover.ToString();
            return response;

        }

        /// <summary>
        /// Moves the rover by command.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <param name="marsRover">The mars rover.</param>
        private void MoveRover(string movements, ref MarsRover marsRover)
        {
                foreach (var movement in movements)
                {
                    var movementItem = Enum.Parse<Movement>(movement.ToString());
                    marsRover.Move(movementItem);
                }
        }



        /// <summary>
        /// Checks if location to deploy is valid.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <exception cref="Exception">Rover outside of bounds</exception>
        private void CheckIfLocationIsValid(int x, int y, Surface surface)
        {
            if (!IsAppropriateLocationRover(x, y, surface))
                throw new Exception("Rover outside of bounds");
        }

        /// <summary>
        /// Determines whether [is appropriate location to deploy rover] [the specified x].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        ///   <c>true</c> if [is appropriate location to deploy rover] [the specified x]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAppropriateLocationRover(int x, int y, Surface surface)
        {
            return x >= 0 && x < surface.Width &&
                   y >= 0 && y < surface.Height;
        }

        /// <summary>
        /// Parses the mars rover query.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="">The .</param>
        private MarsRover ParseMarsRoverQuery(string v1, Surface surface)
        {
            var splitCommand = v1.Split(' ');
            MarsRover marsRover = new MarsRover(int.Parse(splitCommand[0]), int.Parse(splitCommand[1]), Enum.Parse<Direction>(splitCommand[2]), surface);
            return marsRover;


        }

        /// <summary>
        /// Checks the plataeu.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private Plataeu CheckPlataeu(string parameter)
        {
            var splitCommand = parameter.Split(' ');
            Plataeu plataeu = new Plataeu();
            plataeu.Define(int.Parse(splitCommand[0]) + 1, int.Parse(splitCommand[1]) + 1);
            return plataeu;
        }

    }
}
