using EmpSystem.Data;
using EmpSystem.Models;
using EmpSystem.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace EmpSystem.Repository;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly AppDbContext _db;

    public AttendanceRepository(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<AttendanceViewModel> GetAllAsync()
    {
        return _db.Attendances
            .Include(a => a.Employee)
            .Select(a => new AttendanceViewModel
            {
                AttendanceId = a.AttendanceId,
                EmployeeId = a.EmployeeId,
                EmployeeName = a.Employee.FirstName + " " + a.Employee.LastName,
                Date = a.Date,
                Status = a.Status
            });
    }

    public async Task<AttendanceViewModel?> GetByIdAsync(int id)
    {
        var a = await _db.Attendances
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.AttendanceId == id);

        if (a is null) return null;

        return new AttendanceViewModel
        {
            AttendanceId = a.AttendanceId,
            EmployeeId = a.EmployeeId,
            EmployeeName = a.Employee.FirstName + " " + a.Employee.LastName,
            Date = a.Date,
            Status = a.Status
        };
    }

    public async Task AddAsync(AttendanceViewModel model)
    {
        var attendance = new Attendance
        {
            EmployeeId = model.EmployeeId,
            Date = model.Date.Date,   // strip time component
            Status = model.Status
        };
        await _db.Attendances.AddAsync(attendance);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(AttendanceViewModel model)
    {
        var existing = await _db.Attendances.FindAsync(model.AttendanceId);
        if (existing is null) return;

        existing.EmployeeId = model.EmployeeId;
        existing.Date = model.Date.Date;
        existing.Status = model.Status;
        _db.Attendances.Update(existing);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _db.Attendances.FindAsync(id);
        if (existing is null) return;
        _db.Attendances.Remove(existing);
        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Checks whether an attendance record already exists for the given employee on the given date.
    /// Optionally exclude a specific record (for edit scenarios).
    /// </summary>
    public async Task<bool> ExistsAsync(int employeeId, DateTime date, int? excludeId = null)
    {
        var query = _db.Attendances
            .Where(a => a.EmployeeId == employeeId && a.Date == date.Date);

        if (excludeId.HasValue)
            query = query.Where(a => a.AttendanceId != excludeId.Value);

        return await query.AnyAsync();
    }

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        return await _db.Employees
            .Where(e => e.IsActive)
            .OrderBy(e => e.FirstName)
            .ToListAsync();
    }
}
