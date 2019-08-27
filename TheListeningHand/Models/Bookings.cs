using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheListeningHand.Models
{
    public class Booking
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string style { get; set; }
        public string info { get; set; }
        public string stylist { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string starttime { get; set; }
    }
    public class Bookings
    {
        public List<Booking> currentBookings { get; set; }
        public Bookings()
        {
            HttpContext context = HttpContext.Current;
            if (context.Session["bookings"] != null)
            {
                currentBookings = (List<Booking>)context.Session["bookings"];
            }
            else
            {
                currentBookings = new List<Booking>();

                Booking addBooking = new Booking();
                addBooking.name = "Jackie";
                addBooking.phone = "0766554433";
                addBooking.style = "Perm";
                addBooking.info = "Frizzy hair";
                addBooking.stylist = "Tina Sparkle";
                addBooking.date = DateTime.Today.ToShortDateString();
                addBooking.time = "09:00";
                addBooking.starttime = "09:00";
                currentBookings.Add(addBooking);
                addBooking = new Booking();
                addBooking.name = "Sam";
                addBooking.phone = "0700766433";
                addBooking.style = "Wet Cut";
                addBooking.info = "Very thin hair";
                addBooking.stylist = "Tina Sparkle";
                addBooking.date = DateTime.Today.ToShortDateString();
                addBooking.time = "10:00";
                addBooking.starttime = "10:00";
                currentBookings.Add(addBooking);
                addBooking = new Booking();
                addBooking.name = "Amanda";
                addBooking.phone = "07225544353";
                addBooking.style = "Colour treatment";
                addBooking.info = "Second phase";
                addBooking.stylist = "Tina Sparkle";
                addBooking.date = DateTime.Today.ToShortDateString();
                addBooking.time = "12:00";
                addBooking.starttime = "12:00";
                currentBookings.Add(addBooking);
                addBooking = new Booking();
                addBooking.name = "Jackie's Mum";
                addBooking.phone = "0766554433";
                addBooking.style = "Perm";
                addBooking.info = "Frizzy hair";
                addBooking.stylist = "Tina Sparkle";
                addBooking.date = DateTime.Today.ToShortDateString();
                addBooking.time = "14:00";
                addBooking.starttime = "14:00";
                currentBookings.Add(addBooking);
                addBooking = new Booking();
                addBooking.name = "Gita";
                addBooking.phone = "0766554433";
                addBooking.style = "Perm";
                addBooking.info = "Frizzy hair";
                addBooking.stylist = "Tina Sparkle";
                addBooking.date = DateTime.Today.ToShortDateString();
                addBooking.time = "15:00";
                addBooking.starttime = "15:00";
                currentBookings.Add(addBooking);
                addBooking = new Booking();
                addBooking.name = "Gita's Mum";
                addBooking.phone = "0766554433";
                addBooking.style = "Perm";
                addBooking.info = "Frizzy hair";
                addBooking.stylist = "Tina Sparkle";
                addBooking.date = DateTime.Today.AddDays(1).ToShortDateString();
                addBooking.starttime = "09:00";
                addBooking.time = "09:00";
                currentBookings.Add(addBooking);
                addBooking = new Booking();
                addBooking.name = "Mrs Mop";
                addBooking.phone = "0766554433";
                addBooking.style = "Perm";
                addBooking.info = "Frizzy hair";
                addBooking.stylist = "Tina Sparkle";
                addBooking.date = DateTime.Today.AddDays(1).ToShortDateString();
                addBooking.starttime = "10:00";
                addBooking.time = "10:00";
                currentBookings.Add(addBooking);

                context.Session["bookings"] = currentBookings;
            }
        }
        public List<Booking> AddBooking(string name, string phone, string style, string info, string stylist, string date, string time)
        {
            HttpContext context = HttpContext.Current;
            Booking addBooking = new Booking();
            addBooking.name = name;
            addBooking.phone = phone;
            addBooking.style = style;
            addBooking.info = info;
            addBooking.stylist = stylist;
            addBooking.date = date;
            addBooking.time = time;
            currentBookings.Add(addBooking);
            context.Session["bookings"] = currentBookings;
            return currentBookings;
        }
        public List<Booking> GetBookings()
        {
            HttpContext context = HttpContext.Current;
            currentBookings = (List<Booking>)context.Session["bookings"];
            currentBookings = currentBookings.OrderBy(o => o.date).ThenBy(c => c.time).ThenBy(c => c.stylist).ToList();
            return currentBookings;
        }

        public List<string> GetAvailableStylists(string date, string time)
        {

            HttpContext context = HttpContext.Current;
            currentBookings = (List<Booking>)context.Session["bookings"];
            List<string> myStrings = new List<string>();
            string output = "";
            var result = currentBookings.Find(x => x.date == date && x.time == time && x.stylist == "Tina Sparkle");
            if (result == null)
            {
                output = output + " " + "Tina Sparkle";
                myStrings.Add("Tina Sparkle");
            }
            result = currentBookings.Find(x => x.date == date && x.time == time && x.stylist == "Frank Ferret");
            if (result == null)
            {
                output = output + " " + "Frank Ferret";
                myStrings.Add("Frank Ferret");
            }
            result = currentBookings.Find(x => x.date == date && x.time == time && x.stylist == "Marie");
            if (result == null)
            {
                output = output + " " + "Marie";
                myStrings.Add("Marie");
            }
            result = currentBookings.Find(x => x.date == date && x.time == time && x.stylist == "Holly Bush");

            if (result == null)
            {
                output = output + " " + "Holly Bush";
                myStrings.Add("Holly Bush");
            }

            return myStrings;
        }
    }
}
