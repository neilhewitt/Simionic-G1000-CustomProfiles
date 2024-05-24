using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Simionic.Core;
using System;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.IO;
using Microsoft.Azure.Cosmos;
using System.Text.Json;

namespace Simionic.CustomProfiles.FunctionApp
{
    public static class UpsertProfile
    {
        [Function("UpsertProfile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "upsert/{profileId}")] HttpRequest req,
            [CosmosDBInput("%ProfileDB%", "%ProfileContainer%", Connection = "CosmosDBConnection", PartitionKey = "/id")] CosmosClient client,
            ILogger log)
        {
            try
            {
                // this function can be used to clone a profile by specifying a new profileId
                string body = await new StreamReader(req.Body).ReadToEndAsync();
                
                Profile profile = JsonSerializer.Deserialize<Profile>(body);
                profile.LastUpdated = DateTime.UtcNow;
                profile.Id = (string)req.RouteValues["profileId"];

                var response = await client.Container().UpsertItemAsync(profile, new PartitionKey(profile.Id));

                return new OkObjectResult(response.Resource);
                            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while inserting the profile.");
                return new StatusCodeResult(500);
            }
        }
    }
}
