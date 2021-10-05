using System;
using Elsa.SKS.Package.Services.DTOs;
using Elsa.SKS.Package.Services.DTOs.Converters;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Elsa.SKS.Package.Services.Test
{
    public class HopConverterTests : HopConverter
    {
        private readonly HopConverter _converter;
        
        public HopConverterTests()
        {
            _converter = this;
        }

        [Fact]
        public void GivenAJObjectWithHopTypeWareHouse_WhenConvertingToHop_ThenReturnAWarehouse()
        {
            var jObject = new JObject { ["hopType"] = "Warehouse" };
            var result = Create(typeof(Warehouse), jObject);
            result.Should().BeOfType<Warehouse>();
        }

        [Fact]
        public void GivenAJObjectWithHopTypeTruck_WhenConvertingToHop_ThenReturnATruck()
        {
            var jObject = new JObject { ["hopType"] = "Truck" };
            var result = Create(typeof(Truck), jObject);
            result.Should().BeOfType<Truck>();
        }
        
        [Fact]
        public void GivenAJObjectWithHopTypeTransferWarehouse_WhenConvertingToHop_ThenReturnATransferWarehouse()
        {
            var jObject = new JObject { ["hopType"] = "TransferWarehouse" };
            var result = Create(typeof(TransferWarehouse), jObject);
            result.Should().BeOfType<TransferWarehouse>();
        }
        
        [Fact]
        public void GivenAJObjectWithHopTypeNone_WhenConvertingToHop_ThenThrowException()
        {
            var jObject = new JObject { ["hopType"] = "None" };
            
            this.Invoking(x => x.Create(typeof(Warehouse), jObject))
                .Should()
                .Throw<Exception>();
        }
        
        [Fact]
        public void GivenAJObjectHasNoHopType_WhenConvertingToHop_ThenThrowException()
        {
            var jObject = new JObject();
            
            this.Invoking(x => x.Create(typeof(Warehouse), jObject))
                .Should()
                .Throw<Exception>();
        }
        
        [Fact]
        public void GivenAJObjectHasAnInvalidHopType_WhenConvertingToHop_ThenThrowException()
        {
            var jObject = new JObject { ["hopType"] = "NotAHopType" };
            
            this.Invoking(x => x.Create(typeof(Warehouse), jObject))
                .Should()
                .Throw<Exception>();
        }
        
        [Fact]
        public void GivenAJObjectIsNull_WhenConvertingToHop_ThenThrowException()
        {
            JObject jObject = null;
            
            this.Invoking(x => x.Create(typeof(Warehouse), jObject))
                .Should()
                .Throw<Exception>();
        }
    }
}