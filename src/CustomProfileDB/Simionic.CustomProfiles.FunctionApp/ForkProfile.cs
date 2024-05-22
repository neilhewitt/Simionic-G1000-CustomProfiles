﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Simionic.Core;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Cosmos;

namespace Simionic.CustomProfiles.FunctionApp
{
    public static class ForkProfile
    {
        [FunctionName("ForkProfile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "fork/{profileId}")] HttpRequest req,
            string profileId,
            [CosmosDB("%ProfileDB%", "%ProfileContainer%", Connection = "CosmosDBConnection")]
                CosmosClient client,
            ILogger log)
        {
            try
            {
                Profile profile = await client.GetItem<Profile>(profileId);
                profile.ForkedFrom = profileId;
                profile.Id = null;
                var response = await client.Container().CreateItemAsync(profile);
                
                return new OkObjectResult(new { Id = Guid.Parse(response.Resource.Id) });
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while forking the profile.");
                return new StatusCodeResult(500);
            }
        }
    }
}
