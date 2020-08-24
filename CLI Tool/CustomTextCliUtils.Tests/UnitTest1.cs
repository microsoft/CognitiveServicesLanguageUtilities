using Xunit;
using CustomTextCliUtils.ApplicationLayer.Services.Storage;

namespace CustomTextCliToo.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // arrange
            var dir = "./";
            var fileName = "";
            var expected = "asd";

            // act
            var storageService = new LocalStorageService(dir);
            //storageService.ReadFile(fileName);
            var actual = "asd";

            // assert
            Assert.Equal(actual, expected);
        }
    }
}
