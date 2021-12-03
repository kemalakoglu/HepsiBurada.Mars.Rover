using Mars.Rover.Application.Contract.Services;
using Mars.Rover.Domain.Contract.DTO.Navigate;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mars.Rover.Presentation.API.Controllers
{
    public class NavigateController : Controller
    {
        private IApplicationService appService { get; set; }
        public NavigateController(IApplicationService appService)
        {
            this.appService = appService;

        }

        /// <summary>
        /// Navigates the mars rover.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("api/Navigate/NavigateMarsRover")]
        [HttpPost]
        public async Task<IActionResult> NavigateMarsRover([FromBody] NavigateMarsRoverRequestDTO request)
        {
            return Ok(await this.appService.NavigateMarsRover(request));
        }
    }
}
