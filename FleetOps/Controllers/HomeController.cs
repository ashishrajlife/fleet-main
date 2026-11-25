using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FleetOps.Data;

namespace FleetOps.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    public HomeController(ApplicationDbContext db) => _db = db;

    public IActionResult Index() => RedirectToAction(nameof(AdminDashboard));

    [Authorize(Roles = "Administrator")]
public async Task<IActionResult> AdminDashboard()
{
    ViewBag.UserCount = await _db.Users.CountAsync();
    ViewBag.RoleCount = await _db.Roles.CountAsync();
    ViewBag.VehicleCount = await _db.Vehicles.CountAsync();
    ViewBag.ActiveVehicles = await _db.Vehicles.CountAsync(v => v.Status == Models.VehicleStatus.Active);

    ViewBag.RecentUsers = await _db.Users
        .OrderByDescending(u => u.Id)
        .Take(8)
        .ToListAsync();

    var days = Enumerable.Range(0, 7)
        .Select(i => DateTime.UtcNow.Date.AddDays(-6 + i))
        .ToList();
    var rnd = new Random(42);
    ViewBag.TrendLabels = days.Select(d => d.ToString("ddd")).ToList();
    ViewBag.TrendValues = days.Select(_ => rnd.Next(3, 18)).ToList();

    var total = await _db.Vehicles.CountAsync();
    var active = await _db.Vehicles.CountAsync(v => v.Status == Models.VehicleStatus.Active);
    ViewBag.Utilization = total > 0 ? (int)Math.Round((double)active / total * 100) : 0;

    ViewBag.RoleBreakdown = await _db.Roles.Select(r => r.Name).ToListAsync();

    return View();
}

    [AllowAnonymous]
    public IActionResult Privacy() => View();
}