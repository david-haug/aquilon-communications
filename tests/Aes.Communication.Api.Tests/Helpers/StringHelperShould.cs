using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Aes.Communication.Api;
using Aes.Communication.Api.Helpers;

namespace Aes.Communication.Api.Tests.Helpers
{
    public class StringHelperShould
    {

        [Theory]
        [InlineData("","")]
        [InlineData("test1", "test1")]
        [InlineData("Test1", "test1")]
        [InlineData("test1[1]", "test1[1]")]
        [InlineData("Test1[1]", "test1[1]")]
        [InlineData("Test.Test2[4].Test3.Test4Test5[100].Test6Test7.Test8", "test.test2[4].test3.test4Test5[100].test6Test7.test8")]
        public void CreateCamelCasedPath(string path, string expected)
        {
            var actual = StringHelper.CreateCamelCaseObjectPath(path);
            Assert.Equal(expected, actual);

        }

    }
}
