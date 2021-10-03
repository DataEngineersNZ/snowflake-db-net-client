using Xunit;
using Snowflake.Client.Model;
using System;

namespace Snowflake.Client.Tests.UnitTests
{

    public class SnowflakeClientSettingsTest
    {
        [Theory]
        [InlineData("user", "pw", "account")]
        public void AuthInfo_Ctor(string user, string password, string account)
        {
            var authInfo = new AuthInfo(user, password, account);
            var settings = new SnowflakeClientSettings(authInfo);

            Assert.Equal($"{account}.snowflakecomputing.com", settings.UrlInfo.Host);
            Assert.Equal("https", settings.UrlInfo.Protocol);
            Assert.Equal(443, settings.UrlInfo.Port);

            Assert.Equal(user, settings.AuthInfo.User);
            Assert.Equal(password, settings.AuthInfo.Password);
            Assert.Equal(account, settings.AuthInfo.Account);
        }

        [Theory]
        [InlineData("user", "pw", "account")]
        public void AuthInfo_Props(string user, string password, string account)
        {
            var authInfo = new AuthInfo() { User = user, Password = password, Account = account };
            var settings = new SnowflakeClientSettings(authInfo);

            Assert.Equal($"{account}.snowflakecomputing.com", settings.UrlInfo.Host);
            Assert.Equal("https", settings.UrlInfo.Protocol);
            Assert.Equal(443, settings.UrlInfo.Port);

            Assert.Equal(user, settings.AuthInfo.User);
            Assert.Equal(password, settings.AuthInfo.Password);
            Assert.Equal(account, settings.AuthInfo.Account);
        }

        [Fact]
        public void AuthInfo_Ctor_AccountWithUnderscore()
        {
            var authInfo = new AuthInfo("user", "pw", "account_with_undescore");
            var settings = new SnowflakeClientSettings(authInfo);

            Assert.Equal("account-with-undescore.snowflakecomputing.com", settings.UrlInfo.Host);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AuthInfo_Ctor_EmptyAccount(string account)
        {
            var authInfo = new AuthInfo("user", "pw", account);

            Assert.Throws<ArgumentException>(() => new SnowflakeClientSettings(authInfo));
        }

        [Theory]
        [InlineData("west-us-2", "azure")]
        [InlineData("east-us-2", "azure")]
        [InlineData("us-gov-virginia", "azure")]
        [InlineData("canada-central", "azure")]
        [InlineData("west-europe", "azure")]
        [InlineData("switzerland-north", "azure")]
        [InlineData("southeast-asia", "azure")]
        [InlineData("australia-east", "azure")]
        [InlineData("us-east-2", "aws")]
        [InlineData("us-east-1-gov", "aws")]
        [InlineData("ca-central-1", "aws")]
        [InlineData("eu-west-2", "aws")]
        [InlineData("ap-northeast-1", "aws")]
        [InlineData("ap-south-1", "aws")]
        [InlineData("us-central1", "gcp")]
        [InlineData("europe-west2", "gcp")]
        [InlineData("europe-west4", "gcp")]
        public void AuthInfo_Ctor_Regions(string region, string expectedCloud)
        {
            var authInfo = new AuthInfo("user", "pw", "account", region);
            var settings = new SnowflakeClientSettings(authInfo);

            Assert.Equal($"account.{region}.{expectedCloud}.snowflakecomputing.com", settings.UrlInfo.Host);
        }

        [Theory]
        [InlineData("us-east-1")]
        [InlineData("eu-west-1")]
        [InlineData("eu-central-1")]
        [InlineData("ap-southeast-1")]
        [InlineData("ap-southeast-2")]
        public void AuthInfo_Ctor_Unique_Regions(string region)
        {
            var authInfo = new AuthInfo("user", "pw", "account", region);
            var settings = new SnowflakeClientSettings(authInfo);

            Assert.Equal($"account.{region}.snowflakecomputing.com", settings.UrlInfo.Host);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("us-west-2")]
        public void AuthInfo_Ctor_Default_Region(string region)
        {
            var authInfo = new AuthInfo("user", "pw", "account", region);
            var settings = new SnowflakeClientSettings(authInfo);

            Assert.Equal($"account.snowflakecomputing.com", settings.UrlInfo.Host);
        }

        [Theory]
        [InlineData("ca-central-1.aws")]
        [InlineData("europe-west4.gcp")]
        public void AuthInfo_Ctor_RegionWithCloud(string region)
        {
            var authInfo = new AuthInfo("user", "pw", "account", region);
            var settings = new SnowflakeClientSettings(authInfo);

            Assert.Equal($"account.{region}.snowflakecomputing.com", settings.UrlInfo.Host);
        }

        [Fact]
        public void UrlInfo_Ctor()
        {
            var urlInfo = new UrlInfo("account-url.snowflakecomputing.com");
            var settings = new SnowflakeClientSettings(new AuthInfo("user", "pw", "account"), null, urlInfo);

            Assert.Equal("account-url.snowflakecomputing.com", settings.UrlInfo.Host);
            Assert.Equal("https", settings.UrlInfo.Protocol);
            Assert.Equal(443, settings.UrlInfo.Port);
        }

        [Fact]
        public void UrlInfo_Props()
        {
            var urlInfo = new UrlInfo() { Host = "account-url.snowflakecomputing.com" };
            var settings = new SnowflakeClientSettings(new AuthInfo("user", "pw", "account"), null, urlInfo);

            Assert.Equal("account-url.snowflakecomputing.com", settings.UrlInfo.Host);
            Assert.Equal("https", settings.UrlInfo.Protocol);
            Assert.Equal(443, settings.UrlInfo.Port);
        }

        [Fact]
        public void UrlInfo_Explicit_Host()
        {
            var authInfo = new AuthInfo("user", "pw", "account-auth");
            var urlInfo = new UrlInfo("account-url.snowflakecomputing.com");
            var settings = new SnowflakeClientSettings(authInfo, null, urlInfo);

            Assert.Equal("account-url.snowflakecomputing.com", settings.UrlInfo.Host);
        }

        [Fact]
        public void UrlInfo_Explicit_Host_Account_WithUnderscore()
        {
            var authInfo = new AuthInfo("user", "pw", "account");
            var urlInfo = new UrlInfo("account_with_undescore.snowflakecomputing.com");
            var settings = new SnowflakeClientSettings(authInfo, null, urlInfo);

            Assert.Equal($"account-with-undescore.snowflakecomputing.com", settings.UrlInfo.Host);
        }

        [Theory]
        [InlineData("https://account-1.snowflakecomputing.com", "https", 443)]
        [InlineData("http://account-2.snowflakecomputing.com", "http", 80)]
        public void UrlInfo_Uri(string url, string expectedProtocol, int expectedPort)
        {
            var authInfo = new AuthInfo("user", "pw", "account");
            var urlInfo = new UrlInfo(new Uri(url));
            var settings = new SnowflakeClientSettings(authInfo, null, urlInfo);

            Assert.Equal(url.Replace(expectedProtocol + "://", ""), settings.UrlInfo.Host);
            Assert.Equal(expectedPort, settings.UrlInfo.Port);
            Assert.Equal(expectedProtocol, settings.UrlInfo.Protocol);
        }
    }
}