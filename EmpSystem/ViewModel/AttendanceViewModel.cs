using System.ComponentModel.DataAnnotations;
using EmpSystem.Models;

namespace EmpSystem.ViewModel;

public class AttendanceViewModel
{
    public int AttendanceId { get; set; }

    [Required(ErrorMessage = "Employee is required")]
    [Display(Name = "Employee")]
    public int EmployeeId { get; set; }

    // For display purposes in the view
    public string? EmployeeName { get; set; }

    [Required(ErrorMessage = "Date is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Attendance Date")]
    public DateTime Date { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Status is required")]
    [Display(Name = "Status")]
    public AttendanceStatus Status { get; set; }
}
