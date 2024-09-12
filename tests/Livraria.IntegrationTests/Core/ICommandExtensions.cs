using System.Text;
using Livraria.Core.Domain;
using Newtonsoft.Json;

namespace Livraria.IntegrationTests.Core;

public static class ICommandExtensions
{
    public static StringContent ToPayload(this RequestBase requestBase)
    {
        return new StringContent(JsonConvert.SerializeObject(requestBase), Encoding.UTF8, "application/json");
    }
}