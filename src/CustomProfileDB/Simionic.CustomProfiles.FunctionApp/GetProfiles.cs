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
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;

namespace Simionic.CustomProfiles.FunctionApp
{

    public static class GetProfiles
    {
        [FunctionName("GetProfiles")]
        public async static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "profiles")] HttpRequest req,
            [CosmosDB("%ProfileDB%", "%ProfileContainer%", Connection = "CosmosDBConnection")] CosmosClient client,
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
