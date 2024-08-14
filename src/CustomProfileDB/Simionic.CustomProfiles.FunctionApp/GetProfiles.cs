using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Simionic.Core;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Simionic.CustomProfiles.FunctionApp
{

    public static class GetProfiles
    {
        [Function("GetProfiles")]
        public async static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "profiles")] HttpRequest req,
            [CosmosDBInput("G1000ProfileStore", "Profiles", Connection = "CosmosDBConnection", PartitionKey = "/id")] CosmosClient client, 
            ILogger log)
        {
            try
            {
                ProfileSummary[] profiles = await client.GetItems<ProfileSummary>("SELECT c.id, c.AircraftType, c.Engines, c.Name, c.LastUpdated, c.IsPublished, c.Notes, c.Owner FROM c");
                
                return new OkObjectResult(profiles);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while getting the profile list.");
                return new StatusCodeResult(500);
            }
        }
    }
}
