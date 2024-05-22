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
    public static class InsertProfile
    {
        [FunctionName("InsertProfile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "insert")] HttpRequest req,
            [CosmosDB("%ProfileDB%", "%ProfileContainer%", Connection = "CosmosDBConnection")]
                CosmosClient client,
            ILogger log)
        {
            try
            {
                string body = new StreamReader(req.Body).ReadToEnd();
                Profile profile = JsonConvert.DeserializeObject<Profile>(body);
                profile.LastUpdated = DateTime.UtcNow;
                profile.Id = Guid.NewGuid().ToString(); // Cosmos DB will not generate the ID for us

                await client.Container().CreateItemAsync(profile);
                
                return new OkObjectResult(profile);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while inserting the profile.");
                return new StatusCodeResult(500);
            }
        }
    }
}
