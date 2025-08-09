/Controllers/InsureeController.cs
 [HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create(Insuree insuree)
{
    if (ModelState.IsValid)
    {
        // 1. Base monthly quote
        decimal quote = 50m;

        // 2. Age calculation
        var today = DateTime.Today;
        int age = today.Year - insuree.DateOfBirth.Year;
        if (insuree.DateOfBirth > today.AddYears(-age)) age--;

        if (age <= 18)
        {
            quote += 100;
        }
        else if (age >= 19 && age <= 25)
        {
            quote += 50;
        }
        else
        {
            quote += 25;
        }

        // 3. Car year
        if (insuree.CarYear < 2000)
        {
            quote += 25;
        }
        else if (insuree.CarYear > 2015)
        {
            quote += 25;
        }

        // 4. Car make/model
        if (insuree.CarMake != null && insuree.CarMake.ToLower() == "porsche")
        {
            quote += 25;

            if (insuree.CarModel != null && insuree.CarModel.ToLower() == "911 carrera")
            {
                quote += 25;
            }
        }

        // 5. Speeding tickets
        quote += insuree.SpeedingTickets * 10;

        // 6. DUI
        if (insuree.DUI)
        {
            quote *= 1.25m;
        }

        // 7. Coverage type
        if (insuree.CoverageType)
        {
            quote *= 1.5m;
        }

        // Set final quote value
        insuree.Quote = Math.Round(quote, 2);

        // Save to DB
        db.Insurees.Add(insuree);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    return View(insuree);
}
@*<div class="form-group">
    @Html.LabelFor(model => model.Quote, htmlAttributes: new { @class = "control-label col-md-2" })
    <div class="col-md-10">
        @Html.EditorFor(model => model.Quote, new { htmlAttributes = new { @class = "form-control" } })
        @Html.ValidationMessageFor(model => model.Quote, "", new { @class = "text-danger" })
    </div>
</div>*@
public ActionResult Admin()
{
    var insurees = db.Insurees.ToList();
    return View(insurees);
}
@model IEnumerable<YourProjectNamespace.Models.Insuree>

@{
    ViewBag.Title = "Admin Quotes View";
}

<h2>Admin Quotes View</h2>

<table class="table">
    <tr>
        <th>First Name</th>
        <th>Last Name</th>
        <th>Email Address</th>
        <th>Quote</th>
    </tr>

@foreach (var item in Model)
{
    <tr>
        <td>@item.FirstName</td>
        <td>@item.LastName</td>
        <td>@item.EmailAddress</td>
        <td>$@item.Quote</td>
    </tr>
}
</table>
/AssignmentPart1
/AssignmentPart2
/AssignmentPart3
/AssignmentPart4
    /Controllers
        InsureeController.cs
    /Models
        Insuree.cs
    /Views
        /Insuree
            Create.cshtml
            Admin.cshtml
            ...
/App_Data/
    Insurance.mdf
    Insurance_log.ldf