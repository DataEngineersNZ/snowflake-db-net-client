﻿using Xunit;
using System;
using System.Globalization;

namespace Snowflake.Client.Tests.UnitTests
{

    public class SnowflakeTypesConverterTest
    {
        [Theory]
        [InlineData("4133980799.999999900", "2100-12-31 23:59:59.9999999")]
        [InlineData("7258159353.445566700", "2200-01-01 11:22:33.4455667")]
        [InlineData("253402300799.999999900", "9999-12-31 23:59:59.9999999")]
        [InlineData("380218800.666666600", "1982-01-18 16:20:00.6666666")]
        public void SnowflakeTimestampNtz_ToDateTime(string sfTimestampNtz, string expectedDateTimeString)
        {
            var expectedDateTime = DateTime.ParseExact(expectedDateTimeString, "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
            var convertedResult = SnowflakeTypesConverter.ConvertToDateTime(sfTimestampNtz, "timestamp_ntz");

            Assert.Equal(expectedDateTime, convertedResult);
        }

        [Theory]
        [InlineData("4133980799.999999900", "2100-12-31 23:59:59.9999999")]
        [InlineData("7258159353.445566700", "2200-01-01 11:22:33.4455667")]
        [InlineData("253402300799.999999900", "9999-12-31 23:59:59.9999999")]
        [InlineData("380218800.666666600", "1982-01-18 16:20:00.6666666")]
        public void SnowflakeTime_ToDateTime(string sfTime, string expectedDateTimeString)
        {
            var expectedDateTime = DateTime.ParseExact(expectedDateTimeString, "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
            var convertedResult = SnowflakeTypesConverter.ConvertToDateTime(sfTime, "time"); // same conversion as for "timestamp_ntz"

            Assert.Equal(expectedDateTime, convertedResult);
        }

        [Theory]
        [InlineData("47846", "2100-12-31")]
        [InlineData("84006", "2200-01-01")]
        [InlineData("2932896", "9999-12-31")]
        [InlineData("4400", "1982-01-18")]
        public void SnowflakeDate_ToDateTime(string sfDate, string expectedDateTimeString)
        {
            var expectedDateTime = DateTime.ParseExact(expectedDateTimeString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var convertedResult = SnowflakeTypesConverter.ConvertToDateTime(sfDate, "date");

            Assert.Equal(expectedDateTime, convertedResult);
        }

        [Theory]
        [InlineData("2100-12-31 23:59:59.9999999", "4133980799999999900")]
        [InlineData("2200-01-01 11:22:33.4455667", "7258159353445566700")]
        [InlineData("9999-12-31 23:59:59.9999999", "253402300799999999900")]
        [InlineData("1982-01-18 16:20:00.6666666", "380218800666666600")]
        public void DateTime_ToSnowflakeTimestampNtz(string dateTimeString, string expectedSnowflakeTimestampNtz)
        {
            var dateTime = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
            var convertedResult = SnowflakeTypesConverter.ConvertToTimestampNtz(dateTime);

            Assert.Equal(expectedSnowflakeTimestampNtz, convertedResult);
        }

        [Theory]
        [InlineData("2100-12-31 23:59:59.9999999", "4133980799999999900 1440")]
        [InlineData("2200-01-01 11:22:33.4455667", "7258159353445566700 1440")]
        [InlineData("9999-12-31 23:59:59.9999999", "253402300799999999900 1440")]
        [InlineData("1982-01-18 16:20:00.6666666", "380218800666666600 1440")]
        public void DateTimeOffset_ToSnowflakeTimestampTz(string dateTimeString, string expectedSnowflakeTimestampTz)
        {
            var dateTimeOffsetUtc = DateTimeOffset.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var convertedResult = SnowflakeTypesConverter.ConvertToTimestampTz(dateTimeOffsetUtc);

            Assert.Equal(expectedSnowflakeTimestampTz, convertedResult);
        }

        [Theory]
        [InlineData(0, 128, 255, "0080ff")]
        [InlineData(10, 11, 12, "0a0b0c")]
        public void ConvertBytesToHex(byte b1, byte b2, byte b3, string hexExpected)
        {
            var hexResult = SnowflakeTypesConverter.BytesToHex(new byte[] { b1, b2, b3 });
            Assert.Equal(hexExpected, hexResult);
        }

        [Theory]
        [InlineData("0080ff", 0, 128, 255)]
        [InlineData("0a0b0c", 10, 11, 12)]
        public void ConvertHexToBytes(string hex, byte b1, byte b2, byte b3)
        {
            byte[] expectedBytes = new byte[] { b1, b2, b3 };
            var resultBytes = SnowflakeTypesConverter.HexToBytes(hex);

            Assert.Equal(expectedBytes, resultBytes);
        }
    }
}
