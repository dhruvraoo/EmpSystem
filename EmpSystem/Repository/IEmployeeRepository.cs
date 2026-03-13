using EmpSystem.Models;
using EmpSystem.ViewModel;

namespace EmpSystem.Repository;

public interface IEmployeeRepository
{
    Task<EmployeeViewModal> GetByIdAsync(int id);
    IQueryable<EmployeeViewModal> GetAllAsync();
    Task AddAsync(EmployeeViewModal employee);
    Task UpdateAsync(EmployeeViewModal employee);
    Task DeleteAsync(int Id);

    Task <List<Department>> GetAllDepartmentsAsync();
}