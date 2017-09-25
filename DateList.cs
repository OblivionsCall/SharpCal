using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpCal
{
    class DateList
    {
        private int day, month, year;
        private DateList next;

        public DateList()
        {
            day = DateTime.Today.Day;
            month = DateTime.Today.Month;
            year = DateTime.Today.Year;
            next = null;
        }
        public DateList(int day, int month, int year)
        {
            this.day = day;
            this.month = month;
            this.year = year;
            next = null;
        }

        public int getDay()
        {
            return day;
        }
        public int getMonth()
        {
            return month;
        }
        public int getYear()
        {
            return year;
        }
        public DateList getNext()
        {
            return next;
        }

        public void setNext(DateList input)
        {
            next = input;
        }
    }
}
