using System;
using System.Threading.Tasks;
using Drop.Application.Commands;
using Drop.Application.Exceptions;
using Drop.Application.Services;
using Drop.Core.Entities;
using Drop.Core.Repositories;
using Drop.Core.ValueObjects;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Drop.Tests.Unit.Services
{
    public class ParcelsServiceTests
    {
        [Fact]
        public async Task get_async_should_return_parcel_dto()
        {
            // Arrange
            var parcel = new Parcel(Guid.NewGuid(), ParcelSize.Large, "test");
            _parcelsRepository.GetAsync(parcel.Id).Returns(parcel);

            // Act
            var parcelDto = await _parcelsService.GetAsync(parcel.Id);

            // Assert
            parcelDto.ShouldNotBeNull();
            parcelDto.Id.ShouldBe(parcel.Id);
            await _parcelsRepository.Received().GetAsync(parcel.Id);
        }
        
        [Fact]
        public async Task add_parcel_should_fail_given_invalid_size()
        {
            var command = new AddParcel(Guid.NewGuid(), "test_size", "test");
            var exception = await Record.ExceptionAsync(async () => await _parcelsService.AddAsync(command));
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidParcelSizeException>();
        }

        // [Theory]
        // [InlineData("a")]
        // [InlineData("b")]
        // [InlineData("c")]
        // [InlineData("")]
        // public void string_test(string s)
        // {
        //     s.ShouldNotBeNullOrWhiteSpace();
        // }
        
        private readonly IParcelsRepository _parcelsRepository;
        private readonly IParcelsService _parcelsService;
        
        public ParcelsServiceTests()
        {
            _parcelsRepository = Substitute.For<IParcelsRepository>();
            _parcelsService = new ParcelsService(_parcelsRepository);
        }
    }
}