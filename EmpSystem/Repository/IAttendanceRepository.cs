using EmpSystem.Models;
using EmpSystem.ViewModel;

namespace EmpSystem.Repository;

public interface IAttendanceRepository
{
    IQueryable<AttendanceViewModel> GetAllAsync();
    Task<AttendanceViewModel?> GetByIdAsync(int id);
    Task AddAsync(AttendanceViewModel model);
    Task UpdateAsync(AttendanceViewModel model);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int employeeId, DateTime date, int? excludeId = null);
    Task<List<Employee>> GetAllEmployeesAsync();
}
