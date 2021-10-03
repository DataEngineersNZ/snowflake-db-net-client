using Xunit;
using System;
using System.Globalization;

namespace Snowflake.Client.Tests.UnitTests
{
    public class ParameterBindingsSingleValuesTest
    {
        [Theory]
        [InlineData(-42, typeof(int), "FIXED")]
        [InlineData(42, typeof(uint), "FIXED")]
        [InlineData(-42, typeof(sbyte), "FIXED")]
        [InlineData(42, typeof(byte), "FIXED")]
        [InlineData(-42, typeof(short), "FIXED")]
        [InlineData(42, typeof(ushort), "FIXED")]
        [InlineData(-42, typeof(long), "FIXED")]
        [InlineData(42, typeof(ulong), "FIXED")]
        [InlineData(42.5, typeof(float), "REAL")]
        [InlineData(-42.5, typeof(double), "REAL")]
        [InlineData(42.5, typeof(decimal), "REAL")]
        public void BuildParameters_Numeric(object objectValue, Type type, string bindingType)
        {
            var value = Convert.ChangeType(objectValue, type);
            var binding = ParameterBinder.BuildParameterBindings(value);

            Assert.Single(binding);
            Assert.True(binding.ContainsKey("1"));
            Assert.Equal(bindingType, binding["1"].Type);

            var stringValue = string.Format(CultureInfo.InvariantCulture, "{0}", objectValue);
            Assert.Equal(stringValue, binding["1"].Value);
        }

        [Theory]
        [InlineData(true, "BOOLEAN")]
        [InlineData(false, "BOOLEAN")]
        public void BuildParameters_Bool(bool value, string bindingType)
        {
            var binding = ParameterBinder.BuildParameterBindings(value);

            Assert.Single(binding);
            Assert.True(binding.ContainsKey("1"));
            Assert.Equal(bindingType, binding["1"].Type);
            Assert.Equal(value.ToString(), binding["1"].Value);
        }

        [Theory]
        [InlineData("sometext", "TEXT")]
        [InlineData("", "TEXT")]
        public void BuildParameters_String(string value, string bindingType)
        {
            var binding = ParameterBinder.BuildParameterBindings(value);

            Assert.Single(binding);
            Assert.True(binding.ContainsKey("1"));
            Assert.Equal(bindingType, binding["1"].Type);
            Assert.Equal(value, binding["1"].Value);
        }

        [Fact]
        public void BuildParameters_Guid()
        {
            var guid = Guid.NewGuid();

            var binding = ParameterBinder.BuildParameterBindings(guid);

            var stringValue = string.Format(CultureInfo.InvariantCulture, "{0}", guid);

            Assert.Single(binding);
            Assert.True(binding.ContainsKey("1"));
            Assert.Equal("TEXT", binding["1"].Type);
            Assert.Equal(stringValue, binding["1"].Value);
        }

        [Theory]
        [InlineData(new byte[] { 200, 201, 202 }, "c8c9ca", "BINARY")]
        [InlineData(new byte[] { 0 }, "00", "BINARY")]
        public void BuildParameters_BytesArray(byte[] value, string expectedString, string bindingType)
        {
            var binding = ParameterBinder.BuildParameterBindings(value);

            Assert.Single(binding);
            Assert.True(binding.ContainsKey("1"));
            Assert.Equal(bindingType, binding["1"].Type);
            Assert.Equal(expectedString, binding["1"].Value);
        }

        [Theory]
        [InlineData("2021-06-10 16:17:18.0000000", "1623341838000000000", "TIMESTAMP_NTZ")]
        public void BuildParameters_DateTime(string stringValue, string expectedString, string bindingType)
        {
            var value = DateTime.ParseExact(stringValue, "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);

            var binding = ParameterBinder.BuildParameterBindings(value);

            Assert.Single(binding);
            Assert.True(binding.ContainsKey("1"));
            Assert.Equal(bindingType, binding["1"].Type);
            Assert.Equal(expectedString, binding["1"].Value);
        }

        [Theory]
        [InlineData("2021-06-10 16:17:18.0000000", "1623341838000000000 1440", "TIMESTAMP_TZ")]
        public void BuildParameters_DateTimeOffset(string stringValue, string expectedString, string bindingType)
        {
            var value = DateTimeOffset.ParseExact(stringValue, "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

            var binding = ParameterBinder.BuildParameterBindings(value);

            Assert.Single(binding);
            Assert.True(binding.ContainsKey("1"));
            Assert.Equal(bindingType, binding["1"].Type);
            Assert.Equal(expectedString, binding["1"].Value);
        }
    }
}
