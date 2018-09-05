using Quote_codeFirst_.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Filter = Quote_codeFirst_.Models.Filter;

namespace Quote_codeFirst_.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.YearList = SelectYear();
            ViewBag.MakeList = SelectMake();
            ViewBag.ModelList = SelectModel();
            return View();
        }

        [HttpPost]
        public ActionResult Apply(string firstName, string lastName, string emailAddress, string dateOfBirth, int year,
                                  string make, string model, string DUI, int speedingTicket, string coverage)
        {
            double totalAmount;

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress) ||
                string.IsNullOrEmpty(dateOfBirth) || string.IsNullOrEmpty(year.ToString()) || string.IsNullOrEmpty(make) ||
                string.IsNullOrEmpty(model) || string.IsNullOrEmpty(DUI) || string.IsNullOrEmpty(speedingTicket.ToString()) ||
                string.IsNullOrEmpty(coverage))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                totalAmount = Total(dateOfBirth, year, make, model, speedingTicket, DUI, coverage);
                using (QuoteApplication db = new QuoteApplication())
                {
                    var applicant = new Quote();
                    applicant.FirstName = firstName;
                    applicant.LastName = lastName;
                    applicant.EmailAddress = emailAddress;
                    applicant.DateOfBirth = dateOfBirth;
                    applicant.CarYear = year;
                    applicant.CarMake = make;
                    applicant.CarModel = model;
                    applicant.DUI = DUI;
                    applicant.SpeedingTicket = speedingTicket;
                    applicant.Coverage = coverage;
                    applicant.Amount = totalAmount;

                    db.Quotes.Add(applicant);
                    db.SaveChanges();
                }
                return View("Success");
            }
        }

        public ActionResult Admin()
        {
            using (QuoteApplication db = new QuoteApplication())
            {
                var quotes = db.Quotes;
                var filters = new List<Filter>();
                foreach (var quote in quotes)
                {
                    var filter = new Filter();
                    filter.FirstName = quote.FirstName;
                    filter.LastName = quote.LastName;
                    filter.EmailAddress = quote.EmailAddress;
                    filters.Add(filter);
                }
                return View(filters);
            }
        }

        public static List<int> SelectYear()
        {
            string connectionString = @"Data Source=JANLENOVO\SQLEXPRESS;Initial Catalog=CarDB;Integrated Security=True;
                                    Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                    MultiSubnetFailover=False";
            string queryString = @"Select Year FROM YearMakeModel";
            var yearList = new List<int>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var year = new YearMakeModel();
                    year.Year = Convert.ToInt32(reader["Year"]);
                    yearList.Add(year.Year);
                }
                yearList.Sort();
                var years = new List<int>() { yearList[0] };
                for (var i = 1; i < yearList.Count; i++)
                {
                    if (yearList[i] != yearList[i - 1])
                    {
                        years.Add(yearList[i]);
                    }
                }
                return years;
            }
        }

        public static List<string> SelectMake()
        {
            string connectionString = @"Data Source=JANLENOVO\SQLEXPRESS;Initial Catalog=CarDB;Integrated Security=True;
                                    Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                    MultiSubnetFailover=False";
            string queryString = @"Select Make FROM YearMakeModel";
            var makeList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var make = new YearMakeModel();
                    make.Make = reader["Make"].ToString();
                    makeList.Add(make.Make);
                }
                makeList.Sort();
                var makes = new List<string>() { makeList[0] };
                for (var i = 1; i < makeList.Count; i++)
                {
                    if (makeList[i] != makeList[i - 1])
                    {
                        makes.Add(makeList[i]);
                    }
                }
                return makes;
            }
        }

        public static List<string> SelectModel()
        {
            string connectionString = @"Data Source=JANLENOVO\SQLEXPRESS;Initial Catalog=CarDB;Integrated Security=True;
                                    Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                    MultiSubnetFailover=False";
            string queryString = @"Select Model FROM YearMakeModel";
            var modelList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var model = new YearMakeModel();
                    model.Model = reader["Model"].ToString();
                    modelList.Add(model.Model);
                }
                modelList.Sort();
                var models = new List<string>() { modelList[0] };
                for (var i = 1; i < modelList.Count; i++)
                {
                    if (modelList[i] != modelList[i - 1])
                    {
                        models.Add(modelList[i]);
                    }
                }
                return models;
            }
        }

        public static double Total(string birthDate, int year, string make, string model, int ticket, string DUI, string coverage)
        {
            double total = 50;
            DateTime dateOfBirth = Convert.ToDateTime(birthDate);
            int ticketAmount = ticket * 10;
            int age = CalculateAge(dateOfBirth);
            if (age < 18)
            {
                total += 100;
            }
            else if (age > 100 || age < 25)
            {
                total += 25;
            }
            if (year < 2000 || year > 2015) total += 25;
            if (make.ToLower() == "porsche") total += 25;
            if (make.ToLower() == "porsche" && model.ToLower() == "911 carrera") total += 25;
            total += ticketAmount;
            if (DUI == "true") total += (total * .25);
            if (coverage == "full") total += (total * .50);
            return total;
        }

        //Returns the age based on birthdate
        public static int CalculateAge(DateTime birthDay)
        {
            int years = DateTime.Now.Year - birthDay.Year;
            if ((birthDay.Month > DateTime.Now.Month) || (birthDay.Month == DateTime.Now.Month && birthDay.Day > DateTime.Now.Day))
                years--;
            return years;
        }
    }
}