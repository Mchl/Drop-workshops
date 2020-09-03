using System;

namespace Drop.Application.Commands
{
    // Behavior-centric
    public class AddParcel
    {
        public Guid Id { get; set; }
        public string Size { get; set; }
        public string Address { get; set; }
    }
}