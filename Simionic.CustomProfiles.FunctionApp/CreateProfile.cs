using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Simionic.CustomProfiles.Core;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Simionic.CustomProfiles.FunctionApp
{
    public static class CreateProfile
    {
        [FunctionName("CreateProfile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "create")] HttpRequest req,
            [CosmosDB("%ProfileDB%", "%ProfileContainer%", ConnectionStringSetting = "CosmosDBConnection")]
                DocumentClient client,
            ILogger log)
        {
            try
            {
                Profile profile = new Profile() { LastUpdated = DateTime.UtcNow };
                profile.LastUpdated = DateTime.UtcNow;

                var response = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(Helper.ProfileDB, Helper.ProfileContainer), profile);                
                return new OkObjectResult(new { Id = Guid.Parse(response.Resource.Id) });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
