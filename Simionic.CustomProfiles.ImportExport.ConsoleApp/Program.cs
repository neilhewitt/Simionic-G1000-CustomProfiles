// See https://aka.ms/new-console-template for more information
using Simonic.CustomProfiles.ImportExport;

Console.WriteLine("Hello, World!");

CustomProfileDB db = new CustomProfileDB(@"C:\projects\SimionicCustomProfiles\SimionicCustomProfiles\Simonic.CustomProfiles.ImportExport\SampleDB\ACCustom.db");
string folderPath = AppDomain.CurrentDomain.BaseDirectory;
db.Save(folderPath);

//Aircraft aircraft = db.Aircraft.First();
//foreach (ConfigItem configItem in aircraft.ConfigItems)
//{
//    Console.WriteLine($"{aircraft.Name}::{configItem.Name} = {configItem.Value}");
//}

Console.ReadLine();
