using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpSystem.Models;

public class Attendance
{
    [Key]
    public int AttendanceId { get; set; }

    [Required]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [Required]
    public AttendanceStatus Status { get; set; }

    // Navigation property
    public Employee Employee { get; set; } = null!;
}
