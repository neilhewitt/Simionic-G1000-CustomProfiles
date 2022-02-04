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

namespace Simionic.CustomProfiles.FunctionApp
{
    public static class UpdateProfile
    {
        [FunctionName("UpdateProfile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "update/{profileId}")] HttpRequest req,
            Guid profileId,
            [CosmosDB("%ProfileDB%", "%ProfileContainer%", ConnectionStringSetting = "CosmosDBConnection")]
                DocumentClient client,
            ILogger log)
        {
            try
            {
                Profile profile = JsonConvert.DeserializeObject<Profile>(req.Form["profile"]);
                await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(Helper.ProfileDB, Helper.ProfileContainer), profile);
                return new OkObjectResult("OK");
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { Status = 500, Error = ex.Message });
            }
        }
    }
}
