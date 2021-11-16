using System;
using Xunit;
using Function.Domain.Models;
using Function.Domain.Services;

namespace UnitTests.Function.Domain.Services
{
    public class FinhubDataMapperTests
    {
        [Fact]
        public void MapToStockData_ShouldCorrectlyMapTheInput()
        {
            //arrange
            var finHubStockDataInput = new FinhubStockData(){
                o = 111.1M,
                h = 222.2M,
                l = 333.3M,
                c = 444.4M,
                pc = 555.5M
            };

            var sut = new FinhubDataMapper();

            //act
            var result = sut.MapToStockData(finHubStockDataInput);

            //assert
            Assert.Equal(111.1M, result.Open);
            Assert.Equal(222.2M, result.High);
            Assert.Equal(333.3M, result.Low);
            Assert.Equal(444.4M, result.Current);
            Assert.Equal(555.6M, result.PreviousClose);
        }
    }
}

