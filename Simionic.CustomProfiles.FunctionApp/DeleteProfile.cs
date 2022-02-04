using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
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
            [CosmosDB("%ProfileDB%", "%ProfileContainer%", ConnectionStringSetting = "CosmosDBConnection")]
                DocumentClient client,
            ILogger log)
        {
            try
            {
                var uri = UriFactory.CreateDocumentUri(Helper.ProfileDB, Helper.ProfileContainer, profileId);
                await client.DeleteDocumentAsync(uri, new RequestOptions() { PartitionKey = new PartitionKey(profileId) });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { Status = 500, Error = ex.Message, Id = profileId });
            }

            return new StatusCodeResult(200);
        }
    }
}
