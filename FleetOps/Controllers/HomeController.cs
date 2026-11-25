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
        ViewBag.RecentUsers = await _db.Users
            .OrderByDescending(u => u.Id)
            .Take(8)
            .ToListAsync();

        // Weekly signup trend — last 7 days (fallback when no users)
        var since = DateTimeOffset.UtcNow.AddDays(-6);
        var signupsByDay = await _db.Users
            .Where(u => true) // Identity has no CreatedAt by default; replace with real column later
            .GroupBy(u => u.Id.Substring(0, 1))
            .Select(g => new { Key = g.Key, Count = g.Count() })
            .ToListAsync();

        var days = Enumerable.Range(0, 7)
            .Select(i => DateTime.UtcNow.Date.AddDays(-6 + i))
            .ToList();
        var rnd = new Random(42);
        ViewBag.TrendLabels = days.Select(d => d.ToString("ddd")).ToList();
        ViewBag.TrendValues = days.Select(_ => rnd.Next(3, 18)).ToList(); // placeholder until you have trips table

        // Fleet utilization split (placeholder until vehicles/trips tables exist)
        ViewBag.Utilization = 68; // percent active vs idle

        // Latest roles breakdown
        ViewBag.RoleBreakdown = await _db.Roles.Select(r => r.Name).ToListAsync();

        return View();
    }

    [AllowAnonymous]
    public IActionResult Privacy() => View();
}