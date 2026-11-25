using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetOps.Models;

public enum VehicleType { Truck, Van, Car, Bus, Bike }
public enum FuelType { Petrol, Diesel, Electric, CNG, Hybrid }
public enum VehicleStatus { Active, InMaintenance, Retired }

public class Vehicle
{
    public int Id { get; set; }

    [Required, StringLength(20)]
    [Display(Name = "Registration number")]
    public string RegistrationNumber { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Make { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Model { get; set; } = string.Empty;

    [Range(1900, 2100)]
    public int Year { get; set; } = DateTime.UtcNow.Year;

    [Required]
    [Display(Name = "Vehicle type")]
    public VehicleType VehicleType { get; set; } = VehicleType.Truck;

    [Required]
    [Display(Name = "Fuel type")]
    public FuelType FuelType { get; set; } = FuelType.Diesel;

    [Column(TypeName = "decimal(10,2)")]
    [Display(Name = "Capacity (kg)")]
    public decimal? CapacityKg { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    [Display(Name = "Odometer (km)")]
    public decimal OdometerKm { get; set; }

    [Required]
    public VehicleStatus Status { get; set; } = VehicleStatus.Active;

    [DataType(DataType.Date)]
    [Display(Name = "Insurance expiry")]
    public DateOnly? InsuranceExpiry { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fitness expiry")]
    public DateOnly? FitnessExpiry { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Purchase date")]
    public DateOnly? PurchaseDate { get; set; }

    [Display(Name = "Assigned driver")]
    public string? AssignedDriverId { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}