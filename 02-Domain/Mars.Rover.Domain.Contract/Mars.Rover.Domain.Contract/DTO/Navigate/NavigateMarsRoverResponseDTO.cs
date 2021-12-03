using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mars.Rover.Domain.Contract.DTO.Navigate
{
    [DataContract]
    public class NavigateMarsRoverResponseDTO
    {
        [DataMember]
        public string ResponseString { get; set; }
    }
}
