using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simionic.CustomProfiles.FunctionApp
{
    public static class CosmosClientExtensions
    {
        public static Container Container(this CosmosClient client) => client.GetDatabase(Helper.ProfileDB).GetContainer(Helper.ProfileContainer);
        
        public async static Task<T[]> GetItems<T>(this CosmosClient client, string query) where T : class
        {
            using (var iterator = client.Container().GetItemQueryIterator<T>(
                    "SELECT c.id, c.AircraftType, c.Engines, c.Name, c.LastUpdated, c.IsPublished, c.Notes, c.Owner FROM c"
                    ))
            {
                if (iterator.HasMoreResults)
                {
                    var feed = await iterator.ReadNextAsync();
                    return feed.Resource.ToArray();
                }

                return null;
            }
        }

        public async static Task<T> GetItem<T>(this CosmosClient client, string id) where T : class
        {
            var itemResult = await client.Container().ReadItemAsync<T>(id, new PartitionKey(id));
            return itemResult.Resource;
        }
    }
}
