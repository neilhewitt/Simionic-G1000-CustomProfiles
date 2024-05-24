using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Simionic.Core;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Azure.Cosmos;

namespace Simionic.CustomProfiles.FunctionApp
{
    public static class DeleteProfile
    {
        [Function("DeleteProfile")]
        public async static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "delete/{profileId}")] HttpRequest req,
            string profileId,
            [CosmosDBInput("%ProfileDB%", "%ProfileContainer%", Connection = "CosmosDBConnection", PartitionKey = "/id")] CosmosClient client,
            ILogger log)
        {
            try
            {
                await client.Container().DeleteItemAsync<Profile>(profileId, new PartitionKey(profileId));
                return new OkResult();
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while getting the profile.");
                return new StatusCodeResult(500);
            }
        }
    }
}
