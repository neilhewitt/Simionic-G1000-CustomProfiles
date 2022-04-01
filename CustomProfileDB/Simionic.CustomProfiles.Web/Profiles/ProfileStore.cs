using Simionic.Core;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Simionic.CustomProfiles.Web
{
    public static class ProfileStore
    {
        public static async Task<Profile> Insert(Profile profile)
        {
            try
            {
                profile.Owner.Name = User.Name;
                profile.Owner.Id = User.OwnerId;
                profile.IsPublished = false;

                HttpResponseMessage response = await HttpClientFactory.Client.PostAsJsonAsync<Profile>($"/api/insert", profile);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ProfileUpdateException($"Unable to create profile. See ResponseMessage for details.", response);
                }

                profile = await response.Content.ReadFromJsonAsync<Profile>();
                return profile;
            }
            catch (HttpRequestException ex)
            {
                throw new ProfileStoreException($"Unable to create profile. Cannot fetch new profile Id from database. Status code was { (ex.StatusCode.HasValue ? ex.StatusCode.Value.ToString() : "unknown") }.", ex);
            }
        }

        public static async Task Update(Profile profile)
        {
            profile.LastUpdated = DateTime.Now;
            HttpResponseMessage response = await HttpClientFactory.Client.PostAsJsonAsync<Profile>($"/api/update/{profile.Id}", profile);
            if (!response.IsSuccessStatusCode)
            {
                throw new ProfileUpdateException($"Unable to update profile. See ResponseMessage for details.", response);
            }
        }

        public static async Task Delete(Profile profile)
        {
            profile.LastUpdated = DateTime.Now;
            HttpResponseMessage response = await HttpClientFactory.Client.GetAsync($"/api/delete/{profile.Id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new ProfileDeleteException($"Unable to delete profile. See ResponseMessage for details.", response);
            }
        }

        public static async Task<ProfileSummaryList> GetProfileSummaries()
        {
            try
            {
                IEnumerable<ProfileSummary> profiles = await HttpClientFactory.Client.GetFromJsonAsync<IEnumerable<ProfileSummary>>($"/api/profiles");
                return new ProfileSummaryList(profiles);
            }
            catch (HttpRequestException ex)
            {
                throw new ProfileStoreException($"Unable to get profile list. Status code was { (ex.StatusCode.HasValue ? ex.StatusCode.Value.ToString() : "unknown") }.", ex);
            }
        }

        public static async Task<Profile> GetAsync(string id)
        {
            try
            {
                Profile profile = await HttpClientFactory.Client.GetFromJsonAsync<Profile>($"/api/profile/{id}");
                profile.FixUpGauges(); // HACK, but we cannot remove without amending all existing profile JSON in the DB - job TODO
                return profile;
            }
            catch (HttpRequestException ex)
            {
                throw new ProfileStoreException($"Unable to get profile {id}. Status code was { (ex.StatusCode.HasValue ? ex.StatusCode.Value.ToString() : "unknown") }.", ex);
            }
        }
    }
}
