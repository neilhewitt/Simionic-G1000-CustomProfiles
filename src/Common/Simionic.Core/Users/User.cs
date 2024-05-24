using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simionic.Core.Users
{
    public class User
    {
        [JsonPropertyName("id")] // need this for CosmosDB to work properly
        public Guid Id { get; set; }
        public string Identity { get; init; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
