﻿using Microsoft.AspNetCore.Http;
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
                var response = await client.UpsertDocumentAsync("%ProfileCollectionUri%", profile);
                return new OkObjectResult(new { Id = Guid.Parse(response.Resource.Id) });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { Status = 500, Error = ex.Message });
            }
        }
    }
}
