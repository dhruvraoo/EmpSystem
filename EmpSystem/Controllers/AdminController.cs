using EmpSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmpSystem.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    // GET: /Admin — list all HR users
    public async Task<IActionResult> Index()
    {
        var hrUsers = await _userManager.GetUsersInRoleAsync("HR");
        return View(hrUsers);
    }

    // GET: /Admin/CreateHR
    [HttpGet]
    public IActionResult CreateHR()
    {
        return View();
    }

    // POST: /Admin/CreateHR
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateHR(CreateHRUserViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        await _userManager.AddToRoleAsync(user, "HR");
        TempData["Success"] = $"HR user '{model.Email}' created successfully.";
        return RedirectToAction(nameof(Index));
    }

    // POST: /Admin/DeleteUser
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound();

        await _userManager.DeleteAsync(user);
        TempData["Success"] = $"User deleted.";
        return RedirectToAction(nameof(Index));
    }
}
