using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public static class GetProfile
    {
        [FunctionName("GetProfile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "profile/{profileId}")] 
                HttpRequest req,
            string profileId,
            [CosmosDB("%ProfileDB%", "%ProfileContainer%", Id = "{profileId}", PartitionKey = "{profileId}", ConnectionStringSetting = "CosmosDBConnection")]
                DocumentClient client,
            ILogger log)
        {
            try
            {
                Profile profile = await client.ReadDocumentAsync<Profile>(
                    UriFactory.CreateDocumentUri(Helper.ProfileDB, Helper.ProfileContainer, profileId),
                    new RequestOptions() { PartitionKey = new PartitionKey(profileId) }
                    );
                return new OkObjectResult(profile);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
