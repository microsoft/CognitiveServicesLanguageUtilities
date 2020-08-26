using CustomTextCliUtils.ApplicationLayer.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CustomTextCliUtils.Tests.Utilities
{
    class Utilities
    {
        public static void AssertThrows(
            Exception expectedException,
            Action testCode)
        {
            var type = expectedException.GetType();
            var exception = Assert.Throws(type, testCode);
            if (type == typeof(CliException))
            {
                var expected = expectedException as CliException;
                var actual = exception as CliException;
                Assert.Equal(expected.ExceptionCode, actual.ExceptionCode);
                Assert.Equal(expectedException.Message, actual.Message);
            }
        }

        public static async Task AssertThrowsAsync(
            Exception expectedException,
            string exceptionMessage,
            Func<Task> testCode)
        {
            var type = expectedException.GetType();
            var exception = await Assert.ThrowsAsync(type, testCode).ConfigureAwait(false);
            if (type == typeof(CliException))
            {
                var expected = expectedException as CliException;
                var actual = exception as CliException;
                Assert.Equal(expected.ExceptionCode, actual.ExceptionCode);
                Assert.Equal(exceptionMessage, actual.Message);
            }
        }
    }
}
