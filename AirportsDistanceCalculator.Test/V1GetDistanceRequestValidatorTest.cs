using AirportsDistanceCalculator.ApplicationService;
using AirportsDistanceCalculator.Core;
using AirportsDistanceCalculator.Infrastructure;
using AirportsDistanceCalculator.Web.Request;
using AirportsDistanceCalculator.Web.Validators;
using FakeItEasy;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using Xunit;


namespace AirportsDistanceCalculator.Tests
{
    public class V1GetDistanceRequestValidatorTest
    {
        private V1GetDistanceRequestValidator _validator = new V1GetDistanceRequestValidator();
        [Fact]
        public void ShouldReturnTrueForValidIataCode()
        {
            //Arrange

            V1GetDistanceRequest validRequest1 = new V1GetDistanceRequest() { ArrivalIata = "AAA", DepartureIata = "EEE" };
            V1GetDistanceRequest validRequest2 = new V1GetDistanceRequest() { ArrivalIata = "DME", DepartureIata = "LED" };
            V1GetDistanceRequest validRequest3 = new V1GetDistanceRequest() { ArrivalIata = "hel", DepartureIata = "VkO" };
            V1GetDistanceRequest validRequest4 = new V1GetDistanceRequest() { ArrivalIata = "leD", DepartureIata = "vkO" };

            //Act

            var result1 = _validator.Validate(validRequest1).IsValid;
            var result2 = _validator.Validate(validRequest2).IsValid;
            var result3 = _validator.Validate(validRequest3).IsValid;
            var result4 = _validator.Validate(validRequest4).IsValid;

            //Assert

            Assert.True(result1);
            Assert.True(result2);
            Assert.True(result3);
            Assert.True(result4);

        }
        [Fact]
        public void ShouldReturnFalseForInvalidIataCode()
        {
            //Arrange

            V1GetDistanceRequest invalidRequest1 = new V1GetDistanceRequest() { ArrivalIata = "AAAa", DepartureIata = "EEEw" };
            V1GetDistanceRequest invalidRequest2 = new V1GetDistanceRequest() { ArrivalIata = "DME", DepartureIata = "123" };
            V1GetDistanceRequest invalidRequest3 = new V1GetDistanceRequest() { ArrivalIata = "Хел", DepartureIata = "VkO" };
            V1GetDistanceRequest invalidRequest4 = new V1GetDistanceRequest() { ArrivalIata = "w", DepartureIata = "vkO" };

            //Act

            var result1 = _validator.Validate(invalidRequest1).IsValid;
            var result2 = _validator.Validate(invalidRequest2).IsValid;
            var result3 = _validator.Validate(invalidRequest3).IsValid;
            var result4 = _validator.Validate(invalidRequest4).IsValid;

            //Assert

            Assert.False(result1);
            Assert.False(result2);
            Assert.False(result3);
            Assert.False(result4);
        }
    }
}
