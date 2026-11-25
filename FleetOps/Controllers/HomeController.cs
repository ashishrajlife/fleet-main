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

        return View();
    }

    [AllowAnonymous]
    public IActionResult Privacy() => View();
}