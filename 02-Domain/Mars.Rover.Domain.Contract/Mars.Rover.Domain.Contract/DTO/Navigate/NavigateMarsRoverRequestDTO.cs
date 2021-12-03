using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mars.Rover.Domain.Contract.DTO.Navigate
{
    [DataContract]
    public class NavigateMarsRoverRequestDTO
    {
        [DataMember]
        public List<string> Parameters { get; set; }
    }
}
