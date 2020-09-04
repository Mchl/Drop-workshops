using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Drop.Api;
using Drop.Application.Commands;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Drop.Tests.Integration.Controllers
{
    public class ParcelsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        [Fact]
        public async Task add_parcel_should_return_location_header()
        {
            var command = new AddParcel(Guid.NewGuid(), "large", "test");
            var response = await _client.PostAsync("api/parcels", GetContent(command));
            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            response.Headers.Location.ShouldNotBeNull();
            response.Headers.Location.ToString().ShouldBe($"http://localhost/api/Parcels/{command.Id}");
        }
        
        private static StringContent GetContent(object command) 
            => new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
        
        private readonly HttpClient _client;

        public ParcelsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }
    }
}