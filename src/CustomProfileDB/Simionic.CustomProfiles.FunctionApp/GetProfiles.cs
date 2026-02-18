using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Simionic.Core;
using System.Threading.Tasks;

namespace Simionic.CustomProfiles.FunctionApp;

public class GetProfiles
{
    [Function("GetProfiles")]
    public async static Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "profiles")] HttpRequest req,
        [CosmosDBInput("%ProfileDB%", "%ProfileContainer%", Connection = "CosmosDBConnection", PartitionKey = "/id")] CosmosClient client,
        ILogger log)
    {
        Profile[] profiles = await client.GetItems<Profile>("SELECT * FROM c");

        return new OkObjectResult(profiles);
    }
}