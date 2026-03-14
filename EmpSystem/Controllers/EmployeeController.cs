using Microsoft.AspNetCore.Mvc;
using EmpSystem.Models;
using EmpSystem.Repository;
using EmpSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmpSystem.Controllers;

[Authorize]
public class EmployeeController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IActionResult> Index(string searchString, string sortOrder, int pageNumber, string currentFilter)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["DateOfBirthSortParm"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";
        ViewData["IsActiveSortParam"] = sortOrder == "isactive_asc" ? "isactive_desc" : "isactive_asc";

        if (searchString != null)
            pageNumber = 1;
        else
            searchString = currentFilter;

        ViewData["CurrentFilter"] = searchString;

        var employees = _employeeRepository.GetAllAsync();

        if (!string.IsNullOrEmpty(searchString))
            employees = employees.Where(e => e.FirstName.Contains(searchString) || e.LastName.Contains(searchString));

        employees = sortOrder switch
        {
            "name_desc"     => employees.OrderByDescending(e => e.FirstName),
            "date_asc"      => employees.OrderBy(e => e.DateOfBirth),
            "date_desc"     => employees.OrderByDescending(e => e.DateOfBirth),
            "isactive_desc" => employees.OrderByDescending(e => e.IsActive),
            "isactive_asc"  => employees.OrderBy(e => e.IsActive),
            _               => employees.OrderBy(e => e.FirstName)
        };

        if (pageNumber < 1) pageNumber = 1;
        int pageSize = 5;
        return View(await PaginatedList<EmployeeViewModal>.CreateAsync(employees, pageNumber, pageSize));
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        await PopulateDepartmentDropdownAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(EmployeeViewModal model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDepartmentDropdownAsync();
            return View(model);
        }
        await _employeeRepository.AddAsync(model);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        await PopulateDepartmentDropdownAsync();
        var employee = await _employeeRepository.GetByIdAsync(id);
        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EmployeeViewModal employee)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDepartmentDropdownAsync();
            return View(employee);
        }
        await _employeeRepository.UpdateAsync(employee);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        await _employeeRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    private async Task PopulateDepartmentDropdownAsync(int? selectedId = null)
    {
        var departments = await _employeeRepository.GetAllDepartmentsAsync();

        // HR users cannot assign employees to the HR department
        if (User.IsInRole("HR"))
            departments = departments.Where(d =>
                !d.DepartmentName.Equals("HR", StringComparison.OrdinalIgnoreCase)).ToList();

        ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", selectedId);
    }
}