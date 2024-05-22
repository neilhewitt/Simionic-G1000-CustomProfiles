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
    public static class UpdateProfile
    {
        [FunctionName("UpdateProfile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "update/{profileId}")] HttpRequest req,
            Guid profileId,
            [CosmosDB("%ProfileDB%", "%ProfileContainer%", Connection = "CosmosDBConnection")] CosmosClient client,
            ILogger log)
        {
            try
            {
                Profile profile = JsonConvert.DeserializeObject<Profile>(new StreamReader(req.Body).ReadToEnd());
                profile.LastUpdated = DateTime.UtcNow;
                
                await client.Container().UpsertItemAsync(profile);
                
                return new OkObjectResult("OK");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while updating the profile.");
                return new ObjectResult(new { Status = 500, Error = ex.Message });
            }
        }
    }
}
