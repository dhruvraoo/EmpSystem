using EmpSystem.Repository;
using EmpSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmpSystem.Controllers;

[Authorize(Roles = "Admin,HR")]
public class AttendanceController : Controller
{
    private readonly IAttendanceRepository _attendanceRepo;

    public AttendanceController(IAttendanceRepository attendanceRepo)
    {
        _attendanceRepo = attendanceRepo;
    }

    // GET: /Attendance
    public IActionResult Index()
    {
        var records = _attendanceRepo.GetAllAsync();
        return View(records);
    }

    // GET: /Attendance/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var record = await _attendanceRepo.GetByIdAsync(id);
        if (record is null) return NotFound();
        return View(record);
    }

    // GET: /Attendance/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateEmployeeDropdownAsync();
        return View(new AttendanceViewModel { Date = DateTime.Today });
    }

    // POST: /Attendance/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AttendanceViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateEmployeeDropdownAsync();
            return View(model);
        }

        if (await _attendanceRepo.ExistsAsync(model.EmployeeId, model.Date))
        {
            ModelState.AddModelError(string.Empty,
                "An attendance record for this employee on this date already exists.");
            await PopulateEmployeeDropdownAsync();
            return View(model);
        }

        await _attendanceRepo.AddAsync(model);
        TempData["Success"] = "Attendance record created successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Attendance/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var record = await _attendanceRepo.GetByIdAsync(id);
        if (record is null) return NotFound();
        await PopulateEmployeeDropdownAsync(record.EmployeeId);
        return View(record);
    }

    // POST: /Attendance/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AttendanceViewModel model)
    {
        if (id != model.AttendanceId) return BadRequest();

        if (!ModelState.IsValid)
        {
            await PopulateEmployeeDropdownAsync(model.EmployeeId);
            return View(model);
        }

        if (await _attendanceRepo.ExistsAsync(model.EmployeeId, model.Date, model.AttendanceId))
        {
            ModelState.AddModelError(string.Empty,
                "An attendance record for this employee on this date already exists.");
            await PopulateEmployeeDropdownAsync(model.EmployeeId);
            return View(model);
        }

        await _attendanceRepo.UpdateAsync(model);
        TempData["Success"] = "Attendance record updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Attendance/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var record = await _attendanceRepo.GetByIdAsync(id);
        if (record is null) return NotFound();
        return View(record);
    }

    // POST: /Attendance/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _attendanceRepo.DeleteAsync(id);
        TempData["Success"] = "Attendance record deleted.";
        return RedirectToAction(nameof(Index));
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    private async Task PopulateEmployeeDropdownAsync(int? selectedId = null)
    {
        var employees = await _attendanceRepo.GetAllEmployeesAsync();
        ViewBag.Employees = new SelectList(
            employees.Select(e => new { e.EmployeeId, FullName = e.FirstName + " " + e.LastName }),
            "EmployeeId", "FullName", selectedId);
    }
}
