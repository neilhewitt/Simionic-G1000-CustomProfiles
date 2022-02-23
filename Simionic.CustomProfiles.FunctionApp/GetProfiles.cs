using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Simionic.CustomProfiles.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simionic.CustomProfiles.FunctionApp
{

    public static class GetProfiles
    {
        [FunctionName("GetProfiles")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "profiles")] HttpRequest req,
            [CosmosDB("%ProfileDB%", "%ProfileContainer%", ConnectionStringSetting = "CosmosDBConnection")]
              DocumentClient client,
            ILogger log)
        {
            try
            {
                ProfileSummary[] profiles = client.CreateDocumentQuery<ProfileSummary>(
                    UriFactory.CreateDocumentCollectionUri(Helper.ProfileDB, Helper.ProfileContainer),
                    "SELECT c.id, c.AircraftType, c.EngineCount, c.Name, c.LastUpdated, c.IsPublished, c.Owner FROM c",
                    new FeedOptions() { EnableCrossPartitionQuery = true }
                    ).ToArray();
                return new OkObjectResult(profiles);
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { Status = 500, Error = ex.Message });
            }
        }
    }
}
