using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpSystem.Models;

public class Employee
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    /// <summary>
    /// Business role: "Admin" or "HR". Separate from ASP.NET Identity roles.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Role { get; set; } = "HR";

    [ForeignKey("Department")]
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    // Navigation to attendance records
    public ICollection<Attendance>? Attendances { get; set; }
}

