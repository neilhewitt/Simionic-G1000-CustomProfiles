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
using Microsoft.Azure.Documents;

namespace Simionic.CustomProfiles.FunctionApp
{
    public static class ForkProfile
    {
        [FunctionName("ForkProfile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "fork/{profileId}")] HttpRequest req,
            string profileId,
            [CosmosDB("%ProfileDB%", "%ProfileContainer%", ConnectionStringSetting = "CosmosDBConnection")]
                DocumentClient client,
            ILogger log)
        {
            try
            {
                Profile profile = await client.ReadDocumentAsync<Profile>(
                    UriFactory.CreateDocumentUri(Helper.ProfileDB, Helper.ProfileContainer, profileId),
                    new RequestOptions() { PartitionKey = new PartitionKey(profileId) }
                    );
                profile.ForkedFrom = profileId;
                profile.Id = null;
                var response = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(Helper.ProfileDB, Helper.ProfileContainer), profile);
                return new OkObjectResult(new { Id = Guid.Parse(response.Resource.Id) });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { Status = 500, Error = ex.Message, Id = profileId });
            }
        }
    }
}
