using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheListeningHand.Models;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Net;
using Newtonsoft.Json;

namespace TheListeningHand.Controllers
{
    public class HomeController : Controller
    {

        public class bookingview
        {
            public string stylist { get; set; }
            public string customer { get; set; }
            public string date { get; set; }
            public string datedetail { get; set; }
            public string time { get; set; }
            public string phone { get; set; }
            public string style { get; set; }
            public string info { get; set; }

        }
        public ActionResult Index()
        {
            TheListeningHand.Models.TheListeningHandEntities context = new TheListeningHandEntities();
            List<stylist> stylists = context.stylists.ToList();
            string myText = "<select id=\"postage\"><option value=\"0\">Please select...</option><option value=\"1\">Anyone Available</option> ";
            foreach (stylist dresser in stylists)
            {
                myText = myText + "<option value=\"" + dresser.id + "\">" + dresser.name + "</option>";
            }
            myText = myText + " </select>";
            ViewBag.dropdowntext = myText;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult Users()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Facts()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        
        public ActionResult Therapeutic()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Testimonials()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Appointment(string stylist)
        {

            List<bookingview> daycal = new List<bookingview>();
            if (stylist == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Stylist = stylist;

            Bookings myBookings = new Bookings();
            List<Booking> thebook = myBookings.GetBookings();

            List<string> myDates = new List<string>();
            List<string> myDays = new List<string>();
            //    myDates.Add(DateTime.Today.ToShortDateString());
            int i = 0;
            while (i <= 6)
            {
                int j = 9;
                TheListeningHand.Models.TheListeningHandEntities context = new TheListeningHandEntities();
                List<availability> slots = context.availabilities.Where(s => s.stylistId.ToString() == stylist && s.dayId == 1).ToList();
                foreach (availability slot in slots)
                {
                    bookingview calEntry = new bookingview();
                    calEntry.date = DateTime.Today.AddDays(i).ToShortDateString();
                    if (i == 0)
                    {
                        calEntry.datedetail = "Today";
                    }
                    else
                    {
                        calEntry.datedetail = DateTime.Today.AddDays(i).DayOfWeek.ToString();
                    }
                    var timeStr = slot.time.ToString() + ":00";
                    if (j < 10)
                    {
                        timeStr = "0" + timeStr;
                    }
                    calEntry.time = timeStr;
                    calEntry.stylist = stylist;
                    calEntry.customer = "";
                    var result = thebook.Find(x => x.date == DateTime.Today.AddDays(i).ToShortDateString() && x.time == timeStr && x.stylist == stylist);

                    if (result != null)
                    {
                        calEntry.customer = result.name;
                        calEntry.phone = result.phone;
                        calEntry.style = result.style;
                        calEntry.info = result.info;
                    }
                    daycal.Add(calEntry);
                    j++;
                }

       //         while (j <= 17)
       //         {
       //             bookingview calEntry = new bookingview();
       //             calEntry.date = DateTime.Today.AddDays(i).ToShortDateString();
       //             if (i == 0)
       //             {
       //                 calEntry.datedetail = "Today";
       //             }
       //             else
       //             {
       //                 calEntry.datedetail = DateTime.Today.AddDays(i).DayOfWeek.ToString();
       //             }
       //             var timeStr = j.ToString() + ":00";
       //             if (j == 9)
       //             {
       //                 timeStr = "0" + timeStr;
       //             }
       //             calEntry.time = timeStr;
       //             calEntry.stylist = stylist;
       //             calEntry.customer = "";
       //             var result = thebook.Find(x => x.date == DateTime.Today.AddDays(i).ToShortDateString() && x.time == timeStr && x.stylist == stylist);
       //
       //             if (result != null)
       //             {
       //                 calEntry.customer = result.name;
       //                 calEntry.phone = result.phone;
       //                 calEntry.style = result.style;
       //                 calEntry.info = result.info;
       //             }
       //             daycal.Add(calEntry);
       //             j++;
       //         }

                myDates.Add(DateTime.Today.AddDays(i).ToShortDateString());
                i++;

            }
            ViewBag.BookingDates = myDates;

            myDays.Add("Today");
            i = 1;
            while (i <= 6)
            {
                myDays.Add(DateTime.Today.AddDays(i).DayOfWeek.ToString());
                i++;

            }
            ViewBag.BookingDays = myDays;
            return View("Appointment");


        }
        [HttpGet]
        public ActionResult GetUsers(string stylist)
        {
            TheListeningHand.Models.TheListeningHandEntities context = new TheListeningHandEntities();
            List<stylist> stylists = context.stylists.ToList();
            return Json(stylists, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult UpdateUser(int stylist, string arrayhrs )
        {
            dynamic table = JsonConvert.DeserializeObject(arrayhrs);
            string hrs = "";
            int i = 0;
            foreach (var item in  table.hrs)
            {
                i++;
                if (item == true)
                {
                    hrs = hrs + i.ToString() + ",";
                }   
            }
            TheListeningHand.Models.TheListeningHandEntities context = new TheListeningHandEntities();
            List<stylist> stylists = context.stylists.ToList();
            return Json(stylists, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAppointmentsAnyone(string stylist)
        {

            List<bookingview> daycal = new List<bookingview>();
            if (stylist == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Stylist = stylist;

            Bookings myBookings = new Bookings();
            List<Booking> thebook = myBookings.GetBookings();

            List<string> myDates = new List<string>();
            List<string> myDays = new List<string>();
            //    myDates.Add(DateTime.Today.ToShortDateString());
            int i = 0;
            while (i <= 6)
            {
                int j = 9;
                while (j <= 17)
                {
                    bookingview calEntry = new bookingview();
                    calEntry.date = DateTime.Today.AddDays(i).ToShortDateString();
                    if (i == 0)
                    {
                        calEntry.datedetail = "Today";
                    }
                    else
                    {
                        calEntry.datedetail = DateTime.Today.AddDays(i).DayOfWeek.ToString();
                    }
                    var timeStr = j.ToString() + ":00";
                    if (j == 9)
                    {
                        timeStr = "0" + timeStr;
                    }
                    calEntry.time = timeStr;
                    calEntry.stylist = stylist;
                    calEntry.customer = "";
                    //  var result = thebook.Find(x => x.date == DateTime.Today.AddDays(i).ToShortDateString() && x.time == timeStr && x.stylist == stylist);
                    List<string> freestylists = myBookings.GetAvailableStylists(calEntry.date, timeStr);
                    if (freestylists != null && freestylists.Count > 0)
                    {
                        if (j % 2 == 1)
                            calEntry.customer = freestylists.First();
                        else
                            calEntry.customer = freestylists.Last();
                        //  calEntry.phone = result.phone;
                        //  calEntry.style = result.style;
                        //  calEntry.info = result.info;
                    }
                    daycal.Add(calEntry);
                    j++;
                }

                myDates.Add(DateTime.Today.AddDays(i).ToShortDateString());
                i++;

            }
            ViewBag.BookingDates = myDates;

            myDays.Add("Today");
            i = 1;
            while (i <= 6)
            {
                myDays.Add(DateTime.Today.AddDays(i).DayOfWeek.ToString());
                i++;

            }
            ViewBag.BookingDays = myDays;
            return Json(daycal, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAppointments(string stylist)
        {

            List<bookingview> daycal = new List<bookingview>();
            if (stylist == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Stylist = stylist;

            Bookings myBookings = new Bookings();
            List<Booking> thebook = myBookings.GetBookings();

            List<string> myDates = new List<string>();
            List<string> myDays = new List<string>();
            //    myDates.Add(DateTime.Today.ToShortDateString());
            int i = 0;
            while (i <= 6)
            {
                int j = 9;
                TheListeningHand.Models.TheListeningHandEntities context = new TheListeningHandEntities();
                List<availability> slots = context.availabilities.Where(s => s.stylistId.ToString() == "1" ).ToList();
                foreach (availability slot in slots)
                {
                    bookingview calEntry = new bookingview();
                    calEntry.date = DateTime.Today.AddDays(i).ToShortDateString();
                    if (i == 0)
                    {
                        calEntry.datedetail = "Today";
                    }
                    else
                    {
                        calEntry.datedetail = DateTime.Today.AddDays(i).DayOfWeek.ToString();
                    }
                    var timeStr = slot.starttime.ToString() + ":00";
                    if (j < 10)
                    {
                        timeStr = "0" + timeStr;
                    }
                    calEntry.time = timeStr;
                    calEntry.stylist = stylist;
                    calEntry.customer = "";
                    var result = thebook.Find(x => x.date == DateTime.Today.AddDays(i).ToShortDateString() && x.time == timeStr && x.stylist == stylist);

                    if (result != null)
                    {
                        calEntry.customer = result.name;
                        calEntry.phone = result.phone;
                        calEntry.style = result.style;
                        calEntry.info = result.info;
                    }
                    daycal.Add(calEntry);
                    j++;
                }

                myDates.Add(DateTime.Today.AddDays(i).ToShortDateString());
                i++;

            }
            ViewBag.BookingDates = myDates;

            myDays.Add("Today");
            i = 1;
            while (i <= 6)
            {
                myDays.Add(DateTime.Today.AddDays(i).DayOfWeek.ToString());
                i++;

            }
            ViewBag.BookingDays = myDays;
            return Json(daycal, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddAppointment(string name, string phone, string style, string info, string stylist, string date, string time)
        {
            Bookings myBookings = new Bookings();
            myBookings.AddBooking(name, phone, style, info, stylist, date, time);
            //  myBookings.GetBookings();
            return Json(myBookings.currentBookings, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllAppointments()
        {
            Bookings myBookings = new Bookings();
            var thebook = myBookings.GetBookings();
            return Json(thebook, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GoToAppointment(string stylist)
        {
            return RedirectToAction("Appointment", new { stylist = stylist });
        }

        public ActionResult Hairdresser(string stylist)
        {
            List<bookingview> daycal = new List<bookingview>();
            if (stylist == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Stylist = stylist;



            return View();
        }
        public ActionResult Management()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Blog()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Splash()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Anyone(string stylist)
        {

            List<bookingview> daycal = new List<bookingview>();
            if (stylist == null)
            {
                ///   return RedirectToAction("Index");
                stylist = "Anyone Available";
            }
            ViewBag.Stylist = stylist;

            Bookings myBookings = new Bookings();
            List<Booking> thebook = myBookings.GetBookings();

            List<string> myDates = new List<string>();
            List<string> myDays = new List<string>();
            //    myDates.Add(DateTime.Today.ToShortDateString());
            int i = 0;
            while (i <= 6)
            {
                int j = 9;
                while (j <= 17)
                {
                    bookingview calEntry = new bookingview();
                    calEntry.date = DateTime.Today.AddDays(i).ToShortDateString();
                    if (i == 0)
                    {
                        calEntry.datedetail = "Today";
                    }
                    else
                    {
                        calEntry.datedetail = DateTime.Today.AddDays(i).DayOfWeek.ToString();
                    }
                    var timeStr = j.ToString() + ":00";
                    if (j == 9)
                    {
                        timeStr = "0" + timeStr;
                    }
                    calEntry.time = timeStr;
                    calEntry.stylist = stylist;
                    calEntry.customer = "";
                    var result = thebook.Find(x => x.date == DateTime.Today.AddDays(i).ToShortDateString() && x.time == timeStr); // && x.stylist == stylist);

                    //   string freestylists = myBookings.GetAvailableStylists(calEntry.date, timeStr);
                    if (result != null)
                    {
                        calEntry.customer = result.name;
                        //  calEntry.phone = result.phone;
                        //  calEntry.style = result.style;
                        //  calEntry.info = result.info;
                    }
                    daycal.Add(calEntry);
                    j++;
                }

                myDates.Add(DateTime.Today.AddDays(i).ToShortDateString());
                i++;

            }
            ViewBag.BookingDates = myDates;

            myDays.Add("Today");
            i = 1;
            while (i <= 6)
            {
                myDays.Add(DateTime.Today.AddDays(i).DayOfWeek.ToString());
                i++;

            }
            ViewBag.BookingDays = myDays;
            return View("Anyone");

        }
    }
}