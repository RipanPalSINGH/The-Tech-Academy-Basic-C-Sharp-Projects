using System;
using System.ComponentModel.DataAnnotations;

namespace YourNamespace.Models
{
    public class Insuree
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required, EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int CarYear { get; set; }

        [Required]
        public string CarMake { get; set; }

        [Required]
        public string CarModel { get; set; }

        [Required]
        public int SpeedingTickets { get; set; }

        [Required]
        public bool DUI { get; set; }

        [Required]
        public string CoverageType { get; set; } // "full" or "liability"

        public decimal Quote { get; set; }
    }
}
using System;
using System.Linq;
using System.Web.Mvc;  // Or Microsoft.AspNetCore.Mvc if .NET Core
using YourNamespace.Models;

namespace YourNamespace.Controllers
{
    public class InsureeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                decimal quote = 50m;

                int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
                if (insuree.DateOfBirth > DateTime.Now.AddYears(-age)) age--;

                if (age <= 18)
                    quote += 100;
                else if (age >= 19 && age <= 25)
                    quote += 50;
                else if (age >= 26)
                    quote += 25;

                if (insuree.CarYear < 2000)
                    quote += 25;
                if (insuree.CarYear > 2015)
                    quote += 25;

                if (!string.IsNullOrEmpty(insuree.CarMake) && insuree.CarMake.ToLower() == "porsche")
                {
                    quote += 25;
                    if (!string.IsNullOrEmpty(insuree.CarModel) && insuree.CarModel.ToLower() == "911 carrera")
                    {
                        quote += 25;
                    }
                }

                quote += insuree.SpeedingTickets * 10;

                if (insuree.DUI)
                    quote *= 1.25m;

                if (!string.IsNullOrEmpty(insuree.CoverageType) && insuree.CoverageType.ToLower() == "full")
                    quote *= 1.5m;

                insuree.Quote = Math.Round(quote, 2);

                db.Insurees.Add(insuree);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // GET: Insuree/Admin
        public ActionResult Admin()
        {
            var insurees = db.Insurees.ToList();
            return View(insurees);
        }

        // GET: Insuree/Index (optional)
        public ActionResult Index()
        {
            var insurees = db.Insurees.ToList();
            return View(insurees);
        }
    }
}
@model YourNamespace.Models.Insuree

@{
    ViewBag.Title = "Create Insuree";
}

<h2>Create Insuree</h2>

@using (Html.BeginForm())
{
    @Html.AntiForgeryToken()
    
    <div class="form-group">
        @Html.LabelFor(m => m.FirstName)
        @Html.TextBoxFor(m => m.FirstName, new { @class = "form-control" })
        @Html.ValidationMessageFor(m => m.FirstName)
    </div>

    <div class="form-group">
        @Html.LabelFor(m => m.LastName)
        @Html.TextBoxFor(m => m.LastName, new { @class = "form-control" })
        @Html.ValidationMessageFor(m => m.LastName)
    </div>

    <div class="form-group">
        @Html.LabelFor(m => m.EmailAddress)
        @Html.TextBoxFor(m => m.EmailAddress, new { @class = "form-control" })
        @Html.ValidationMessageFor(m => m.EmailAddress)
    </div>

    <div class="form-group">
        @Html.LabelFor(m => m.DateOfBirth)
        @Html.TextBoxFor(m => m.DateOfBirth, "{0:yyyy-MM-dd}", new { @class = "form-control", type = "date" })
        @Html.ValidationMessageFor(m => m.DateOfBirth)
    </div>

    <div class="form-group">
        @Html.LabelFor(m => m.CarYear)
        @Html.TextBoxFor(m => m.CarYear, new { @class = "form-control" })
        @Html.ValidationMessageFor(m => m.CarYear)
    </div>

    <div class="form-group">
        @Html.LabelFor(m => m.CarMake)
        @Html.TextBoxFor(m => m.CarMake, new { @class = "form-control" })
        @Html.ValidationMessageFor(m => m.CarMake)
    </div>

    <div class="form-group">
        @Html.LabelFor(m => m.CarModel)
        @Html.TextBoxFor(m => m.CarModel, new { @class = "form-control" })
        @Html.ValidationMessageFor(m => m.CarModel)
    </div>

    <div class="form-group">
        @Html.LabelFor(m => m.SpeedingTickets)
        @Html.TextBoxFor(m => m.SpeedingTickets, new { @class = "form-control" })
        @Html.ValidationMessageFor(m => m.SpeedingTickets)
    </div>

    <div class="form-group">
        @Html.LabelFor(m => m.DUI)
        @Html.CheckBoxFor(m => m.DUI)
        @Html.ValidationMessageFor(m => m.DUI)
    </div>

    <div class="form-group">
        @Html.LabelFor(m => m.CoverageType)
        @Html.DropDownListFor(m => m.CoverageType, new SelectList(new[] {
            new { Value = "liability", Text = "Liability" },
            new { Value = "full", Text = "Full Coverage" }
        }, "Value", "Text"), "Select Coverage Type", new { @class = "form-control" })
        @Html.ValidationMessageFor(m => m.CoverageType)
    </div>

    @* Hide Quote input, so user doesn't see or edit it *@
    @Html.HiddenFor(m => m.Quote)

    <button type="submit" class="btn btn-primary">Get Quote</button>
}
@model IEnumerable<YourNamespace.Models.Insuree>

@{
    ViewBag.Title = "Admin - All Quotes";
}

<h2>All Insurees & Quotes</h2>

<table class="table table-bordered">
    <thead>
        <tr>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Email Address</th>
            <th>Quote</th>
        </tr>
    </thead>
    <tbody>
        @foreach(var insuree in Model)
        {
            <tr>
                <td>@insuree.FirstName</td>
                <td>@insuree.LastName</td>
                <td>@insuree.EmailAddress</td>
                <td>@insuree.Quote.ToString("C")</td>
            </tr>
        }
    </tbody>
</table>
using System.Data.Entity;
using YourNamespace.Models;

namespace YourNamespace.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<Insuree> Insurees { get; set; }
    }
}
