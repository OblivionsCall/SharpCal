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
    class Calendar
    {
        // class variables
        private DateList first;
        private DateTime today;
        public int month;

        public Calendar()
        {
            today = DateTime.Today;
            month = today.Month;
            first = fillMonth();
        }

        public void addDays(int numDays)
        {
            today = today.AddDays(numDays);

            if (month != today.Month)
                first = fillMonth();

            month = today.Month;
        }

        public void makeToday()
        {
            today = DateTime.Today;

            if (month != today.Month)
                first = fillMonth();

            month = today.Month;
        }

        public void PrintCalendar()
        {
            DateList current = first;
            DateTime firstDay = new DateTime(first.getYear(), first.getMonth(), first.getDay());
            DateTime currentDay;
            int date = (int)firstDay.DayOfWeek;

            PrintMonthYear();

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (i == 0 && j < date)
                    {
                        Console.Write("    ");
                    }
                    else
                    {
                        currentDay = new DateTime(current.getYear(), current.getMonth(), current.getDay());

                        if (current.getDay() < 10)
                        {
                            if (today == currentDay)
                            {
                                Console.Write(" ");
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write(" {0} ", current.getDay());
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.Write("  {0} ", current.getDay());
                            }
                        }
                        else
                        {
                            if(today == currentDay)
                            {
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write(" {0} ", current.getDay());
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.Write(" {0} ", current.getDay());
                            }
                        }

                        current = current.getNext();
                    }

                    if (current == null)
                        break;
                }

                Console.WriteLine();

                if (current == null)
                    break;

                Console.WriteLine();                
            }

            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write("=");
        }

        public DateTime getCurrentDay()
        {
            return today;
        }

        private DateList fillMonth()
        {
            int month = today.Month;
            int year = today.Year;
            DateList first = new DateList(1, month, year);
            DateList current = first;
            DateList temp;

            switch (month)
            {
				// Thirty days hath September, April, June, and November
                case 4:
                case 6:
                case 9:
                case 11:
                    for (int i = 2; i <= 30; i++)
                    {
                        temp = new DateList(i, month, year);
                        current.setNext(temp);
                        current = current.getNext();
                    }
                    break;
				// Except February which has 28 (or 29)
                case 2:
                    if (DateTime.IsLeapYear(year))
                    {
                        for (int i = 2; i <= 29; i++)
                        {
                            temp = new DateList(i, month, year);
                            current.setNext(temp);
                            current = current.getNext();
                        }
                    }
                    else
                    {
                        for (int i = 2; i <= 28; i++)
                        {
                            temp = new DateList(i, month, year);
                            current.setNext(temp);
                            current = current.getNext();
                        }
                    }
                    break;

				// All the rest have 31
                default:
                    for (int i = 2; i <= 31; i++)
                    {
                        temp = new DateList(i, month, year);
                        current.setNext(temp);
                        current = current.getNext();
                    }
                    break;
            }

            return first;
        }
        private void PrintMonthYear()
        {
            string monthYear = "";

            switch(first.getMonth())
            {
                case 1:
                    monthYear = "January";
                    break;
                case 2:
                    monthYear = "February";
                    break;
                case 3:
                    monthYear = "March";
                    break;
                case 4:
                    monthYear = "April";
                    break;
                case 5:
                    monthYear = "May";
                    break;
                case 6:
                    monthYear = "June";
                    break;
                case 7:
                    monthYear = "July";
                    break;
                case 8:
                    monthYear = "August";
                    break;
                case 9:
                    monthYear = "September";
                    break;
                case 10:
                    monthYear = "October";
                    break;
                case 11:
                    monthYear = "November";
                    break;
                case 12:
                    monthYear = "December";
                    break;
                default:
                    monthYear = "Error";
                    break;
            }

            monthYear = monthYear + " " + first.getYear();
            
            int spaces = (Console.WindowWidth / 2) - (monthYear.Length / 2);

            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write("=");

            for (int i = 0; i < spaces; i++)
            {
                Console.Write(" ");
            }

            Console.WriteLine(monthYear);
            Console.WriteLine("Sun Mon Tue Wed Thu Fri Sat");
        }
    }   
}
