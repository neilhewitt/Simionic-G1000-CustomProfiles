using System;
using System.Xml.Linq;
using CoordinateSharp;

namespace Simionic.UserWaypoint.Converter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            args = new string[] { "sample.pln" };

            if (args.Length > 0)
            {
                string pln = args[0];
                if (File.Exists(pln))
                {
                    XDocument plan = XDocument.Load(pln);
                    var waypoints = plan.Descendants().Where(x => x.Name == "ATCWaypoint" && x.Elements().First().Value == "User");
                    int index = 1;
                    foreach (var waypoint in waypoints)
                    {
                        string[] dmsParts = waypoint.Descendants().Skip(1).First().Value.Split(',');
                        string lat = dmsParts[0];
                        string lon = dmsParts[1];                        

                        Coordinate coordinate = Coordinate.Parse($"{lat} {lon}");
                        double latDD = coordinate.Latitude.DecimalDegree;
                        double lonDD = coordinate.Longitude.DecimalDegree;

                        string name = $"WPT{index++}";
                        string comment = waypoint.Attribute("id")?.Value ?? "No waypoint ID";

                        string sql = $"INSERT INTO UserWpt (Id, Comment, Latitude, Longitude) VALUES ('{name}','{comment}','{latDD}','{lonDD}')";
                        Console.WriteLine(sql);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid PLN path.");
                    return;
                }

            }
            else
            {
                Console.WriteLine("No PLN path supplied.");
                return;
            }
        }
    }
}