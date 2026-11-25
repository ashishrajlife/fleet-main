using FleetOps.Data;
using FleetOps.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetOps.Controllers;

[Authorize(Roles = "Administrator,Dispatcher")]
public class VehiclesController : Controller
{
    private readonly ApplicationDbContext _db;
    public VehiclesController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? q, VehicleStatus? status, VehicleType? type)
    {
        var query = _db.Vehicles.AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            query = query.Where(v =>
                v.RegistrationNumber.Contains(q) ||
                v.Make.Contains(q) ||
                v.Model.Contains(q));
        }

        if (status.HasValue) query = query.Where(v => v.Status == status);
        if (type.HasValue) query = query.Where(v => v.VehicleType == type);

        ViewBag.Q = q;
        ViewBag.Status = status;
        ViewBag.Type = type;

        var list = await query
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync();

        return View(list);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
        if (vehicle is null) return NotFound();
        return View(vehicle);
    }

    public IActionResult Create() => View(new Vehicle());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Vehicle model)
    {
        if (await _db.Vehicles.AnyAsync(v => v.RegistrationNumber == model.RegistrationNumber))
            ModelState.AddModelError(nameof(model.RegistrationNumber), "This registration number already exists.");

        if (!ModelState.IsValid) return View(model);

        model.CreatedAt = model.UpdatedAt = DateTimeOffset.UtcNow;
        _db.Vehicles.Add(model);
        await _db.SaveChangesAsync();
        TempData["Ok"] = $"Vehicle {model.RegistrationNumber} created.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var vehicle = await _db.Vehicles.FindAsync(id);
        if (vehicle is null) return NotFound();
        return View(vehicle);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Vehicle model)
    {
        if (id != model.Id) return BadRequest();

        if (await _db.Vehicles.AnyAsync(v => v.RegistrationNumber == model.RegistrationNumber && v.Id != id))
            ModelState.AddModelError(nameof(model.RegistrationNumber), "This registration number already exists.");

        if (!ModelState.IsValid) return View(model);

        var existing = await _db.Vehicles.FindAsync(id);
        if (existing is null) return NotFound();

        existing.RegistrationNumber = model.RegistrationNumber;
        existing.Make = model.Make;
        existing.Model = model.Model;
        existing.Year = model.Year;
        existing.VehicleType = model.VehicleType;
        existing.FuelType = model.FuelType;
        existing.CapacityKg = model.CapacityKg;
        existing.OdometerKm = model.OdometerKm;
        existing.Status = model.Status;
        existing.InsuranceExpiry = model.InsuranceExpiry;
        existing.FitnessExpiry = model.FitnessExpiry;
        existing.PurchaseDate = model.PurchaseDate;
        existing.AssignedDriverId = model.AssignedDriverId;
        existing.Notes = model.Notes;
        existing.UpdatedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync();
        TempData["Ok"] = $"Vehicle {existing.RegistrationNumber} updated.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var vehicle = await _db.Vehicles.FindAsync(id);
        if (vehicle is null) return NotFound();
        return View(vehicle);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var vehicle = await _db.Vehicles.FindAsync(id);
        if (vehicle is not null)
        {
            _db.Vehicles.Remove(vehicle);
            await _db.SaveChangesAsync();
            TempData["Ok"] = $"Vehicle {vehicle.RegistrationNumber} deleted.";
        }
        return RedirectToAction(nameof(Index));
    }
}