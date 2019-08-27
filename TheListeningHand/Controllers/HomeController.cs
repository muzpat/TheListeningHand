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
using System.Configuration;
using MySql.Data.MySqlClient;

namespace TheListeningHand.Controllers
{
    public class HomeController : Controller
    {
        public class aAppointment
        {
            public string dayoftheweek { get; set; }
            public string theday { get; set; }
            public string stylistid { get; set; }
            public string dayId { get; set; }
            public string starttime { get; set; }
            public string name { get; set; }
            public string phone { get; set; }
            public string style { get; set; }
            public string info { get; set; }
            public string stylist { get; set; }
            public string appdate { get; set; }
            public string apptime { get; set; }
        }

        public class Appointmentapage
        {
            public string dayoftheweek { get; set; }
            public string theday { get; set; }
            public string stylistid { get; set; }
            public string dayId { get; set; }
            public string starttime { get; set; }
            public string time { get; set; }
        }
        public class UserDetails
        {
            public string name { get; set; }
            public string email { get; set; }
            public string id { get; set; }
            public string stylistId { get; set; }
            public string dayId { get; set; }
            public string starttime { get; set; }
            public string time { get; set; }

        }
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
            myText = "<select id=\"postage\"><option value=\"0\">Please select...</option><option value=\"1\">New User</option> ";
            foreach (stylist dresser in stylists)
            {
                myText = myText + "<option value=\"" + dresser.id + "\">" + dresser.name + "</option>";
            }
            myText = myText + " </select>";
            ViewBag.userdropdown = myText;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult Users(string stylist)
        {
            ViewBag.Message = "Your application description page.";
            ViewBag.username = stylist;
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
                        calEntry.style = result.style == null ? "n/a" : result.style;
                        calEntry.info = result.info == null ? "n/a" : result.info;
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
            return View("Appointment");


        }
        [HttpGet]
        public ActionResult AddUsers(string stylistname, string email)
        {
            TheListeningHand.Models.TheListeningHandEntities context = new TheListeningHandEntities();
            string connstr = System.Configuration.ConfigurationManager.ConnectionStrings["name"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand("spAddUser", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                        //   cmd.Parameters.Add(new MySqlParameter("stylist", MySqlDbType.Int32, myStylistid));
                        MySqlParameter[] pms = new MySqlParameter[2];
                        pms[0] = new MySqlParameter("name", MySqlDbType.VarChar, 40);
                        pms[0].Value = stylistname;
                        pms[1] = new MySqlParameter("email", MySqlDbType.VarChar, 40);
                        pms[1].Value = email;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "spAddUser";
                        cmd.Parameters.AddRange(pms);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    var str = ex.Message;
                }
            }
            return Json("[]", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAppointmentPage(string stylist)
        {
            TheListeningHand.Models.TheListeningHandEntities context = new TheListeningHandEntities();
            int myStylistid = 1;
            var st = context.stylists.Where(s => s.name == stylist).FirstOrDefault();
            if (st != null)
            {
                myStylistid = (int)st.id;
            }

            string connstr = System.Configuration.ConfigurationManager.ConnectionStrings["name"].ConnectionString;
            MySqlDataReader rdr = null;
            List<Appointmentapage> userlist = new List<Appointmentapage>();
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand("spAppointments", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        conn.Open();
                        //   cmd.Parameters.Add(new MySqlParameter("stylist", MySqlDbType.Int32, myStylistid));
                        MySqlParameter[] pms = new MySqlParameter[1];
                        pms[0] = new MySqlParameter("mykey", MySqlDbType.Int32);
                        pms[0].Value = myStylistid;
                        cmd.Parameters.AddRange(pms);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "spAppointments";
                        rdr = cmd.ExecuteReader();

                        var jsonResult = new System.Text.StringBuilder();
                        if (!rdr.HasRows)
                        {
                            jsonResult.Append("[]");
                        }
                        else
                        {
                            while (rdr.Read())
                            {
                                Appointmentapage userslot = new Appointmentapage();
                                //userslot.name = rdr.GetValue(0).ToString();
                                //userslot.email = rdr.GetValue(1).ToString();
                                //userslot.id = rdr.GetValue(2).ToString();
                                userslot.dayoftheweek = rdr.GetValue(0).ToString();
                                userslot.theday = rdr.GetValue(1).ToString();
                                userslot.stylistid = rdr.GetValue(2).ToString();
                                userslot.dayId = rdr.GetValue(3).ToString();
                                userslot.starttime = rdr.GetValue(4).ToString();
                                userslot.time = rdr.GetValue(5).ToString();
                                userlist.Add(userslot);
                            }            
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    var str = ex.Message;
                }
            }
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetUsers(string stylist)
        {
            TheListeningHand.Models.TheListeningHandEntities context = new TheListeningHandEntities();
            int myStylistid = 1;
            var st = context.stylists.Where(s => s.name == stylist).FirstOrDefault();
            if (st != null)
            {
                myStylistid = (int)st.id;
            }

            string connstr = System.Configuration.ConfigurationManager.ConnectionStrings["name"].ConnectionString;
            MySqlDataReader rdr = null;
            List<UserDetails> userlist = new List<UserDetails>();
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand("spGetUserDetails", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        conn.Open();
                        //   cmd.Parameters.Add(new MySqlParameter("stylist", MySqlDbType.Int32, myStylistid));
                        MySqlParameter[] pms = new MySqlParameter[1];
                        pms[0] = new MySqlParameter("xstylist", MySqlDbType.Int32);
                        pms[0].Value = stylist;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "spGetUserDetails";
                        rdr = cmd.ExecuteReader();

                        var jsonResult = new System.Text.StringBuilder();
                        if (!rdr.HasRows)
                        {
                            jsonResult.Append("[]");
                        }
                        else
                        {
                            while (rdr.Read())
                            {
                                UserDetails userslot = new UserDetails();
                                //userslot.name = rdr.GetValue(0).ToString();
                                //userslot.email = rdr.GetValue(1).ToString();
                                //userslot.id = rdr.GetValue(2).ToString();
                                userslot.stylistId = rdr.GetValue(1).ToString();
                                userslot.dayId = rdr.GetValue(0).ToString();
                                userslot.starttime = rdr.GetValue(2).ToString();
                                userslot.time = rdr.GetValue(3).ToString();
                                userlist.Add(userslot);
                            }
                        }
                        conn.Close();
                    }
                }
                catch  (Exception ex) { 
                    var str =  ex.Message;
                }
            }
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult UpdateUser(int stylist, string arrayhrs, string day )
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
            int dayId = 1;
            if (day == "Sunday")
            {
                dayId = 1;
            }
            else if (day == "Monday")
            {
                dayId = 2;
            }
            else if (day == "Tuesday")
            {
                dayId = 3;
            }
            else if (day == "Wednesday")
            {
                dayId = 4;
            }
            else if (day == "Thursday")
            {
                dayId = 5;
            }
            else if (day == "Friday")
            {
                dayId = 6;
            }
            else if (day == "Saturday")
            {
                dayId = 7;
            }
            //     List<string> data = arrayhrs.Split(',').ToList();
            //     foreach (Boolean yes in arrayhrs)
            //     {
            //
            //
            //     }



            string connstr = System.Configuration.ConfigurationManager.ConnectionStrings["name"].ConnectionString;
            MySqlDataReader rdr = null;
            List<UserDetails> userlist = new List<UserDetails>();
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    hrs = hrs.Remove(hrs.Length - 1, 1);
                    using (MySqlCommand cmd = new MySqlCommand("spUpdateAvailabilty", conn))
                    {
                        MySqlParameter[] pms = new MySqlParameter[3];
                        pms[0] = new MySqlParameter("stylist", MySqlDbType.Int32);
                        pms[0].Value = stylist;
                        pms[1] = new MySqlParameter("hrs", MySqlDbType.VarChar, 100);
                        pms[1].Value = hrs;
                        pms[2] = new MySqlParameter("xayId", MySqlDbType.Int32);
                        pms[2].Value = dayId;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "spUpdateAvailabilty";
                        cmd.Parameters.AddRange(pms);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                    }
                }
                catch (Exception ex)
                {
                    var str = ex.Message;
                }
            }




            TheListeningHand.Models.TheListeningHandEntities context = new TheListeningHandEntities();
        //    context.spUpdateAvailabilty(stylist, hrs, dayId);
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
            List<aAppointment> userlist = new List<aAppointment>();
            TheListeningHand.Models.TheListeningHandEntities context = new TheListeningHandEntities();
            string connstr = System.Configuration.ConfigurationManager.ConnectionStrings["name"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                MySqlDataReader rdr = null;
             
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand("spGetBookings", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //   cmd.Parameters.Add(new MySqlParameter("stylist", MySqlDbType.Int32, myStylistid));
                     //   MySqlParameter[] pms = new MySqlParameter[2];
                     //   pms[0] = new MySqlParameter("name", MySqlDbType.VarChar, 40);
                     //   pms[0].Value = stylistname;
                     //   pms[1] = new MySqlParameter("email", MySqlDbType.VarChar, 40);
                     //   pms[1].Value = email;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "spGetBookings";
                      //  cmd.Parameters.AddRange(pms);
                        conn.Open();
                        rdr = cmd.ExecuteReader();

                        var jsonResult = new System.Text.StringBuilder();
                        if (!rdr.HasRows)
                        {
                            jsonResult.Append("[]");
                        }
                        else
                        {
                            while (rdr.Read())
                            {
                                aAppointment userslot = new aAppointment();
                                userslot.dayoftheweek = rdr.GetValue(0).ToString();
                                userslot.theday = rdr.GetValue(1).ToString();
                                userslot.stylistid = rdr.GetValue(2).ToString();
                                userslot.dayId = rdr.GetValue(3).ToString();
                                userslot.starttime = rdr.GetValue(4).ToString();
                                userslot.name = rdr.GetValue(5).ToString();
                                userslot.phone = rdr.GetValue(6).ToString();
                                userslot.style = rdr.GetValue(7).ToString();
                                userslot.info = rdr.GetValue(8).ToString();
                                userslot.stylist = rdr.GetValue(9).ToString();
                                userslot.appdate = rdr.GetValue(10).ToString();
                                userslot.apptime = rdr.GetValue(11).ToString();
    
                                userlist.Add(userslot);
                            }
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    var str = ex.Message;
                }
            }
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult AddAppointment(string name, string phone, string style, string info, string stylist, string date, string time)
        {
            int iTime = int.Parse(time);
            string connstr = System.Configuration.ConfigurationManager.ConnectionStrings["name"].ConnectionString;
            List<UserDetails> userlist = new List<UserDetails>();
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {

                    using (MySqlCommand cmd = new MySqlCommand("spUpdateAvailabilty", conn))
                    {
                        MySqlParameter[] pms = new MySqlParameter[7];
                        pms[0] = new MySqlParameter("name", MySqlDbType.VarChar, 40);
                        pms[0].Value = name;
                        pms[1] = new MySqlParameter("phone", MySqlDbType.VarChar, 40);
                        pms[1].Value = phone;
                        pms[2] = new MySqlParameter("style", MySqlDbType.VarChar, 40);
                        pms[2].Value = style;
                        pms[3] = new MySqlParameter("info", MySqlDbType.VarChar, 40);
                        pms[3].Value = info;
                        pms[4] = new MySqlParameter("stylist", MySqlDbType.VarChar, 40);
                        pms[4].Value = stylist;
                        pms[5] = new MySqlParameter("date", MySqlDbType.VarChar, 10);
                        pms[5].Value = date;
                        pms[6] = new MySqlParameter("time", MySqlDbType.Int32);
                        pms[6].Value = iTime;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "spAddAppointment";
                        cmd.Parameters.AddRange(pms);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                    }
                }
                catch (Exception ex)
                {
                    var str = ex.Message;
                }
            }
            return Json("[]", JsonRequestBehavior.AllowGet);
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
            if (stylist == null)
            {
                stylist = "Tina Sparkle";
            }
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