// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using Simionic.CustomProfiles.Core;
using Simionic.CustomProfiles.ImportExport;
using System.Net.Http.Json;

Console.WriteLine("Hello, World!");

//CustomProfileDB db = new CustomProfileDB(@"C:\projects\SimionicCustomProfiles\SimionicCustomProfiles\Simonic.CustomProfiles.ImportExport\SampleDB\ACCustom.db");
//db.AddProfile(db.Profiles[3]);
//db.SaveToDatabase();
//string folderPath = AppDomain.CurrentDomain.BaseDirectory;

//Profile first = db.Profiles[1];
//AircraftConfig config = new AircraftConfigBuilder(first, 1).AircraftConfig;
//Profile second = new ProfileBuilder(config).Profile;

//File.WriteAllText("first.json", JsonConvert.SerializeObject(first));
//File.WriteAllText("second.json", JsonConvert.SerializeObject(first));

//Aircraft aircraft = db.Aircraft.First();
//foreach (ConfigItem configItem in aircraft.ConfigItems)
//{
//    Console.WriteLine($"{aircraft.Name}::{configItem.Name} = {configItem.Value}");
//}

Console.ReadLine();

public static class HttpClientFactory
{
#if DEBUG
    public const string BaseAddress = "http://localhost:7071";
    public const string ApiHostKey = null;
#else
        public const string BaseAddress = "https://simionic-g1000-profile-database-functions.azurewebsites.net/";
        public const string ApiHostKey = "RdyGRIvTPLAkTlJGe8f8hOaFtdmZSK3sdcFCeKHf0239EqqwzsUc3w==";
#endif
    public static HttpClient Client
    {
        get
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);
            client.DefaultRequestHeaders.Add("x-functions-key", ApiHostKey);
            return client;
        }
    }

    public static async Task<Profile> Insert(Profile profile)
    {
        try
        {

            HttpResponseMessage response = await HttpClientFactory.Client.PostAsJsonAsync<Profile>($"/api/insert", profile);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Unable to create profile. See ResponseMessage for details.");
            }

            profile = await response.Content.ReadFromJsonAsync<Profile>();
            return profile;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Unable to create profile. Cannot fetch new profile Id from database. Status code was { (ex.StatusCode.HasValue ? ex.StatusCode.Value.ToString() : "unknown") }.", ex);
        }
    }
}
