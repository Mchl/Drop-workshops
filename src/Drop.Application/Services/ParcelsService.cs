using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drop.Application.Commands;
using Drop.Application.DTO;

namespace Drop.Application.Services
{
    public class ParcelsService : IParcelsService
    {
        private static readonly ISet<ParcelDto> Parcels = new HashSet<ParcelDto>();

        public Task<ParcelDto> GetAsync(Guid id) => Task.FromResult(Parcels.SingleOrDefault(p => p.Id == id));
    
        public Task AddAsync(AddParcel request)
        {
            Parcels.Add(new ParcelDto
            {
                Id = request.Id,
                Size = request.Size,
                Address = request.Address,
                State = "new"
            });

            return Task.CompletedTask;
        }
    }
}