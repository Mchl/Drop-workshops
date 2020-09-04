using System;
using Drop.Core.Entities;
using Drop.Core.ValueObjects;

namespace Drop.Infrastructure.Mongo.Documents
{
    internal class ParcelDocument
    {
        public Guid Id { get; set; }
        public ParcelSize Size { get; set; }
        public string Address { get; set; }
        public ParcelState State { get; set; }

        public ParcelDocument()
        {
        }

        public ParcelDocument(Parcel parcel)
        {
            Id = parcel.Id;
            Size = parcel.Size;
            Address = parcel.Address;
            State = parcel.State;
        }
    }
}