
using Snowflake.Client.Tests.IntegrationTests.Models;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Client.Tests.IntegrationTests
{

    public class SnowflakeSessionTest
    {
        private readonly SnowflakeConnectionInfo _conectionInfo;

        public SnowflakeSessionTest()
        {
            var configJson = File.ReadAllText("testconfig.json");
            var testParameters = JsonSerializer.Deserialize<TestConfiguration>(configJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            _conectionInfo = testParameters.Connection;
        }

        [Fact]
        public async Task InitNewSession()
        {
            var snowflakeClient = new SnowflakeClient(_conectionInfo.User, _conectionInfo.Password, _conectionInfo.Account, _conectionInfo.Region);

            var sessionInitialized = await snowflakeClient.InitNewSessionAsync();

            Assert.True(sessionInitialized);
            Assert.NotNull(snowflakeClient.SnowflakeSession);
        }

        [Fact]
        public async Task RenewSession()
        {
            var snowflakeClient = new SnowflakeClient(_conectionInfo.User, _conectionInfo.Password, _conectionInfo.Account, _conectionInfo.Region);

            var sessionInitialized = await snowflakeClient.InitNewSessionAsync();
            var firstSessionToken = snowflakeClient.SnowflakeSession.SessionToken;

            var sessionRenewed = await snowflakeClient.RenewSessionAsync();
            var secondSessionToken = snowflakeClient.SnowflakeSession.SessionToken;

            Assert.True(sessionInitialized);
            Assert.True(sessionRenewed);
            Assert.True(firstSessionToken != secondSessionToken);
        }

        [Fact]
        public async Task CloseSession()
        {
            var snowflakeClient = new SnowflakeClient(_conectionInfo.User, _conectionInfo.Password, _conectionInfo.Account, _conectionInfo.Region);

            var sessionInitialized = await snowflakeClient.InitNewSessionAsync();
            var sessionClosed = await snowflakeClient.CloseSessionAsync();

            Assert.True(sessionInitialized);
            Assert.True(sessionClosed);
            Assert.Null(snowflakeClient.SnowflakeSession);
        }
    }
}
