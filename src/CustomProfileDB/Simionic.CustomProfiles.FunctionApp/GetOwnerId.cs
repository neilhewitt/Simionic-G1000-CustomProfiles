using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Simionic.CustomProfiles.FunctionApp
{
    public static class GetOwnerId
    {
        [Function("GetOwnerId")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "ownerid")] HttpRequest req,
            ILogger log)
        {
            if (!req.Query.TryGetValue("email", out var email))
            {
                return new BadRequestResult();
            }

            string ownerId = Helper.GetOwnerId(email);
            return new OkObjectResult(ownerId);
        }
    }
}
