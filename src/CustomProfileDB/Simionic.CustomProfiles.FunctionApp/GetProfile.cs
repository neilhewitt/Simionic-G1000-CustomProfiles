using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Simionic.Core;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Azure.Cosmos;

namespace Simionic.CustomProfiles.FunctionApp
{
    public static class GetProfile
    {
        [FunctionName("GetProfile")]
        public async static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "profile/{profileId}")] HttpRequest req,
            string profileId,
            [CosmosDB("%ProfileDB%", "%ProfileContainer%", Id = "{profileId}", PartitionKey = "{profileId}", Connection = "CosmosDBConnection")] CosmosClient client,
            ILogger log)
        {
            try
            {
                Profile profile = await client.GetItem<Profile>(profileId);
                
                return new OkObjectResult(profile);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while getting the profile.");
                return new StatusCodeResult(500);
            }
        }
    }
}
