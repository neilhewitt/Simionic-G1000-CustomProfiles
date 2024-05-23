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
    public static class UpsertProfile
    {
        [Function("UpsertProfile")]
        [CosmosDBOutput("%ProfileDB%", "%ProfileContainer%", Connection = "CosmosDBConnection", PartitionKey = "/id")]
        public static async Task<string> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "upsert/{profileId}")] HttpRequest req,
            ILogger log)
        {
            try
            {
                string body = await new StreamReader(req.Body).ReadToEndAsync();
                
                Profile profile = JsonConvert.DeserializeObject<Profile>(body);
                profile.LastUpdated = DateTime.UtcNow;
                profile.Id = (string)req.RouteValues["profileId"];

                return JsonConvert.SerializeObject(profile);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while inserting the profile.");
                return "Error!";
            }
        }
    }
}
