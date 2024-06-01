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
        public static Container ProfileContainer(this CosmosClient client) => client.GetDatabase(Helper.ProfileDB).GetContainer(Helper.ProfileContainer);
        
        public async static Task<T[]> GetItems<T>(this CosmosClient client, string query) where T : class
        {
            using (var iterator = client.ProfileContainer().GetItemQueryIterator<T>(query))
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
            var itemResult = await client.ProfileContainer().ReadItemAsync<T>(id, new PartitionKey(id));
            return itemResult.Resource;
        }
    }
}
