
using Snowflake.Client.Tests.IntegrationTests.Models;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Client.Tests.IntegrationTests
{

    public class SnowflakeQueriesTest
    {
        private readonly SnowflakeClient _snowflakeClient;

        public SnowflakeQueriesTest()
        {
            var configJson = File.ReadAllText("testconfig.json");
            var testParameters = JsonSerializer.Deserialize<TestConfiguration>(configJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            var conectionInfo = testParameters.Connection;

            _snowflakeClient = new SnowflakeClient(conectionInfo.User, conectionInfo.Password, conectionInfo.Account, conectionInfo.Region);
        }

        [Fact]
        public async Task ExecuteScalar_WithResult()
        {
            string result = await _snowflakeClient.ExecuteScalarAsync("SELECT CURRENT_USER();");

            Assert.True(!string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public async Task ExecuteScalar_Null()
        {
            string result = await _snowflakeClient.ExecuteScalarAsync("SELECT 1 WHERE 2 > 3;");

            Assert.Null(result);
        }

        [Fact]
        public async Task Execute()
        {
            // todo: do temporary insert to get affected rows > 0

            long result = await _snowflakeClient.ExecuteAsync("SELECT 1;");

            Assert.True(result == -1);
        }

        [Fact]
        public async Task QueryRawResponse()
        {
            var result = await _snowflakeClient.QueryRawResponseAsync("SELECT CURRENT_USER();");

            Assert.NotNull(result);
        }
    }
}
