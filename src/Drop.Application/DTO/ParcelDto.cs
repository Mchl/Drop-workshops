using System;

namespace Drop.Application.DTO
{
    // Data-centric
    public class ParcelDto
    {
        public Guid Id { get; set; }
        public string Size { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
    }
}