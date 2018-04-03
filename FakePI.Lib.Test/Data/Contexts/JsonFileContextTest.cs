using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FakePI.Lib.Core;
using FakePI.Lib.Data.Contexts;
using FakePI.Lib.Test.Fakes;
using FakePI.Lib.Util.Contracts;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace FakePI.Lib.Test.Data.Contexts
{
    public class JsonFileContextTest
    {
        public JsonFileContextTest()
        {
            Fixture = new Fixture();
            FakeEntities = Fixture.CreateMany<SimpleEntity>().ToList();
            
            FileReaderMock = new Mock<IFileReader>();
            JsonConverterMock = new Mock<IJsonConverter>();
            OptionsMock = new Mock<IOptions<AppConfig>>();
            
            SetupFileReaderMock();
            SetupJsonConverterMock();
            SetupOptionsMock();
            
            Sut = new JsonFileContext<SimpleEntity>(FileReaderMock.Object, JsonConverterMock.Object, OptionsMock.Object);
        }
        
        private Fixture Fixture { get; }
        private ICollection<SimpleEntity> FakeEntities { get; }
        
        private Mock<IFileReader> FileReaderMock { get; }
        private Mock<IJsonConverter> JsonConverterMock { get; }
        private Mock<IOptions<AppConfig>> OptionsMock { get; }
        
        private JsonFileContext<SimpleEntity> Sut { get; }

        [Fact]
        public async Task JsonFileContext_ReturnsSingle()
        {
            var id = FakeEntities.First().Id;
            var result = await Sut.GetAsync(id);

            result.Should().Be(FakeEntities.First());
        }

        [Fact]
        public async Task JsonFileContext_ReturnsMany()
        {
            var result = await Sut.GetAllAsync();

            result.Should().Equal(FakeEntities);
        }

        [Fact]
        public void JsonFileContext_Put_ThrowsException()
        {
            Func<Task<bool>> f = async () => await Sut.PutAsync(FakeEntities.First());
            f.Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void JsonFileContext_PutMany_ThrowsException()
        {
            Func<Task<bool>> f = async () => await Sut.PutManyAsync(FakeEntities);
            f.Should().Throw<NotSupportedException>();
        }
        
        #region Helpers

        private void SetupFileReaderMock()
        {
            
            FileReaderMock.Setup(m => m.FileExists(It.IsAny<string>())).Returns(true);
            FileReaderMock.Setup(m => m.ReadFile(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(FakeEntities));
        }

        private void SetupJsonConverterMock()
        {
            JsonConverterMock
                .Setup(m => m.FromJson<ICollection<SimpleEntity>>(It.IsAny<string>()))
                .Returns(FakeEntities);
        }

        private void SetupOptionsMock()
        {
            var fakeConfig = Fixture.Create<AppConfig>();
            OptionsMock.Setup(m => m.Value).Returns(fakeConfig);
        }
        
        #endregion
    }
}