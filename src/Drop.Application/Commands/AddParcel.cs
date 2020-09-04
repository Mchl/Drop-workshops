using System;

namespace Drop.Application.Commands
{
    // Behavior-centric
    public class AddParcel
    {
        public Guid Id { get; }
        public string Size { get; }
        public string Address { get; }

        public AddParcel(Guid id, string size, string address)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Size = size;
            Address = address;
        }
    }
}