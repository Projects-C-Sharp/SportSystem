using Microsoft.AspNetCore.Mvc;
using SportSistem.Models.Entities;
using SportSistem.Services;
using SportSistem.Services.Interfaces;

namespace SportSistem.Controllers;

public class UserController: Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService) => _userService = userService;

    public async Task<IActionResult> Index()
        => View(await _userService.GetAllAsync());

    public async Task<IActionResult> Details(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return View(user);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        if (!ModelState.IsValid) return View(user);
        var response = await _userService.CreateAsync(user);
        if (response.Success == false) { ModelState.AddModelError("", response.Message ?? "Unknown error"); return View(user); }
        TempData["Success"] = "User registered successfully.";
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return View("Edit", user);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(User user)
    {
        if (!ModelState.IsValid) return View(user);
        var response = await _userService.UpdateAsync(user);
        if (response.Success == false) { ModelState.AddModelError("", response.Message ?? "Unknown error"); return View(user); }
        TempData["Success"] = "User updated successfully.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Destoy(int id)
    {
        var response = await _userService.DeleteAsync(id);
        TempData[response.Success == true ? "Success" : "Error"] =
            response.Success == true ? "User deleted successfully." : (response.Message ?? "Unknown error");
        return RedirectToAction("Index");
    }
}