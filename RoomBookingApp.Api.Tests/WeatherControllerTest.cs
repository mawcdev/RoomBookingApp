using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using RoomBookingApp.Api.Controllers;
using Shouldly;

namespace RoomBookingApp.Api.Tests
{
    public class WeatherControllerTest
    {
        [Fact]
        public void Should_Return_Forecast_Results()
        {
            var loogerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loogerMock.Object);

            var result = controller.Get();

            result.ShouldNotBeNull();
            result.Count().ShouldBeGreaterThan(1);
        }
    }
}