using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace _3_ClientA
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1Async()
        {
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            Assert.True(!disco.IsError);

            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            Assert.True(!tokenResponse.IsError);

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.GetAsync("http://localhost:5001/api/Values?str=������˽�����");
            Assert.True(response.IsSuccessStatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.True("������˽�����" == content);

            //���ֱ�ӷ��� �����AccessToken���ǲ��ܷ��ʵġ���Ϊ���api�Ѿ���
            //var client2 = new HttpClient();
            //var temp = await client2.GetAsync("http://localhost:5001/api/Values?str=������˽�����");
        }
    }
}
