using EmpSystem.Data;
using EmpSystem.Models;
using EmpSystem.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace EmpSystem.Repository;

public class EmployeeRepository: IEmployeeRepository
{
    private readonly AppDbContext _dbContext;
    public EmployeeRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;        
    }
    public async Task<EmployeeViewModal> GetByIdAsync(int id)
    {
        var employee = await _dbContext.Employees.FindAsync(id);
        var employeeViewModal = new EmployeeViewModal
        {
            EmployeeId = employee!.EmployeeId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            DateOfBirth = employee.DateOfBirth,
            Gender = employee.Gender,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            Address = employee.Address,
            IsActive = employee.IsActive,
            DepartmentId = employee.DepartmentId,
            Role = employee.Role,
        };
        return employeeViewModal;
    }

    public IQueryable<EmployeeViewModal> GetAllAsync()
    {
        var employees = _dbContext.Employees.Select(e => new EmployeeViewModal
        {
            EmployeeId = e.EmployeeId,
            FirstName = e.FirstName,
            LastName = e.LastName,
            DateOfBirth = e.DateOfBirth,
            Gender = e.Gender,
            Email = e.Email,
            PhoneNumber = e.PhoneNumber,
            Address = e.Address,
            IsActive = e.IsActive,
            DepartmentId = e.DepartmentId,
            Role = e.Role,
        });
        return employees;
        // List<Employee> employees = await _dbContext.Employees.ToListAsync();
        // List<EmployeeViewModal> employeeViewModals = new List<EmployeeViewModal>();
        //
        // foreach (var employee in employees)
        // {
        //     var employeeModel = new EmployeeViewModal
        //     {
        //         EmployeeId = employee.EmployeeId,
        //         FirstName = employee.FirstName,
        //         LastName = employee.LastName,
        //         DateOfBirth = employee.DateOfBirth,
        //         Gender = employee.Gender,
        //         Email = employee.Email,
        //         PhoneNumber = employee.PhoneNumber,
        //         Address = employee.Address,
        //         IsActive = employee.IsActive,
        //     };
        //     employeeViewModals.Add(employeeModel);
        // }
        //
        // return employeeViewModals;

    }

    public async Task AddAsync(EmployeeViewModal employee)
    {
        var newEmployee = new Employee
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            Gender = employee.Gender,
            DateOfBirth = employee.DateOfBirth,
            Address = employee.Address,
            IsActive = employee.IsActive,
            DepartmentId = employee.DepartmentId,
            Role = employee.Role,
        };
        await _dbContext.Employees.AddAsync(newEmployee);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(EmployeeViewModal employeeUpdated)
    {
        var employee = await _dbContext.Employees.FindAsync(employeeUpdated.EmployeeId);
        if (employee is null) return;
        employee.FirstName = employeeUpdated.FirstName;
        employee.LastName = employeeUpdated.LastName;
        employee.Email = employeeUpdated.Email;
        employee.DateOfBirth = employeeUpdated.DateOfBirth;
        employee.PhoneNumber = employeeUpdated.PhoneNumber;
        employee.Address = employeeUpdated.Address;
        employee.DepartmentId = employeeUpdated.DepartmentId;
        employee.IsActive = employeeUpdated.IsActive;
        employee.Role = employeeUpdated.Role;
        _dbContext.Employees.Update(employee);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int Id)
    {
        var employee =await _dbContext.Employees.FindAsync(Id);
         _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Department>> GetAllDepartmentsAsync()
    {
        return await _dbContext.Departments.ToListAsync();
    }
}