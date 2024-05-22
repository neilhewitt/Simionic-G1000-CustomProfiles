using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Simionic.Core;
using System;
using System.Threading.Tasks;

namespace Simionic.CustomProfiles.FunctionApp
{
    public static class DeleteProfile
    {
        [FunctionName("DeleteProfile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "delete/{profileId}")] HttpRequest req,
            string profileId,
            [CosmosDB("%ProfileDB%", "%ProfileContainer%", Connection = "CosmosDBConnection")]
                CosmosClient client,
            ILogger log)
        {
            try
            {
                await client.Container().DeleteItemAsync<Profile>(profileId, new PartitionKey(profileId));
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while deleting the profile.");
                return new StatusCodeResult(500);
            }

            return new StatusCodeResult(200);
        }
    }
}
