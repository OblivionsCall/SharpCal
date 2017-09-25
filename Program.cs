using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace sharpCal
{
    class Program
    {
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "SharpCal";

        static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/calendar-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
			
            Console.SetWindowSize(29, 40);
            Console.SetBufferSize(29, 40);
            Console.Title = "sharpCal";

            Calendar cal = new Calendar();
            string input = "";
            string calendarName = "primary";

            while (!input.Equals("quit") && !input.Equals("q") && !input.Equals("Quit")) 
            {
                System.Console.Clear();
                cal.PrintCalendar();
                printEvent(credential, cal, calendarName);

                Console.Write(":");
                input = Console.ReadLine();

                // Add Strings here and call cal.addDays(numDaysToAdd)
                // For example "Next year" would call cal.addDays(365)
                switch (input)
                {
                    case "Tomorrow":
                    case "tomorrow":
                        cal.addDays(1);
                        break;
                    case "Yesterday":
                    case "yesterday":
                        cal.addDays(-1);
                        break;
                    case "Today":
                    case "today":
                    case "":
                        cal.makeToday();
                        break;
                    case "help":
                    case "Help":
                        help();
                        break;
                    case "Calendars":
                    case "calendars":
                        calendarName = calendars(credential);
                        break;
                    default:
                        break;
                }
            }
        }

        static void help()
        {
            Console.Clear();
            Console.WriteLine("Available commands");

            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write("-");

            Console.WriteLine("today");
            Console.WriteLine("tomorrow");
            Console.WriteLine("yesterday");
            Console.WriteLine();
            Console.WriteLine("calendars");
            Console.WriteLine();
            Console.WriteLine("Quit, quit, q");
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadKey();
        }

        static string calendars(UserCredential credential)
        {
            Console.Clear();

            // TODO - Insert calender list display code here, return List.name at end.
			// Still trying to figure out how to do this

            return "primary";
        }

        static void printEvent(UserCredential credential, Calendar cal, string calendarName)
        {
            DateTime today = cal.getCurrentDay();
            DateTime tomorrow = cal.getCurrentDay();
            tomorrow = tomorrow.AddDays(1);

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            EventsResource.ListRequest request = service.Events.List(calendarName);
            request.TimeMin = today;
            request.TimeMax = tomorrow;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            Events events = request.Execute();
            Console.WriteLine();

            if (events.Items == null || events.Items.Count == 0)
            {
                Console.WriteLine("No Events today!");
                Console.WriteLine();
            }
            else
            {
                foreach (var eventItem in events.Items)
                {
                    string whenString = eventItem.Start.DateTime.ToString();
                    string when = "";
                    DateTime start;
                    DateTime end;

                    if (!String.IsNullOrEmpty(whenString))
                    {
                        start = (DateTime)eventItem.Start.DateTime;
                        when += start.ToShortTimeString();
                    }

                    whenString = eventItem.End.DateTime.ToString();

                    if (!String.IsNullOrEmpty(whenString))
                    {
                        end = (DateTime)eventItem.End.DateTime;
                        when += " - " + end.ToShortTimeString();
                    }

                    if (when == "")
                        when = "All Day";
                                                            
                    Console.WriteLine(when);
                    Console.WriteLine(eventItem.Summary);
                    Console.WriteLine();
                }
            }

        }
    }
}
