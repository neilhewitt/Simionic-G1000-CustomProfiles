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
using System.IO;

namespace Simionic.CustomProfiles.FunctionApp
{
    public static class InsertProfile
    {
        [FunctionName("InsertProfile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "insert")] HttpRequest req,
            [CosmosDB("%ProfileDB%", "%ProfileContainer%", ConnectionStringSetting = "CosmosDBConnection")]
                DocumentClient client,
            ILogger log)
        {
            try
            {
                string body = new StreamReader(req.Body).ReadToEnd();
                Profile profile = JsonConvert.DeserializeObject<Profile>(body);
                profile.LastUpdated = DateTime.UtcNow;

                var response = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(Helper.ProfileDB, Helper.ProfileContainer), profile);
                profile.Id = response.Resource.Id;
                return new OkObjectResult(profile);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}