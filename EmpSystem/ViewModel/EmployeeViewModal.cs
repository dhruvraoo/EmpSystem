using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EmpSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpSystem.ViewModel;

public class EmployeeViewModal
{
    public EmployeeViewModal()
    {
        
    }
    [Key]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "First Name is required")]
    [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Date of Birth is required")]
    [DataType(DataType.Date)]       
    public DateTime DateOfBirth { get; set; }
    [Required(ErrorMessage = "Gender is required")]
    [DataType(DataType.Text)]
    public string Gender { get; set; }
    [Required(ErrorMessage = "Phone Number is required")]
    [Phone(ErrorMessage = "Invalid Phone Number")]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }
    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Role is required")]
    [Display(Name = "Role")]
    public string Role { get; set; } = "HR";

    [ForeignKey("Department")]
    public int DepartmentId { get; set; } //Foreign Key
    public Department? Department { get; set; } //Reference navigation property
}