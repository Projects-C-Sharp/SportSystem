using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportSistem.Models.Entities;
using SportSistem.Models.Enums;
using SportSistem.Services.Interfaces;

namespace SportSistem.Controllers;

public class SpaceController : Controller
{
    private readonly ISpaceService _spaceService;

    public SpaceController(ISpaceService spaceService) => _spaceService = spaceService;

    public async Task<IActionResult> Index(SpaceType? type)
    {
        var spaces = type.HasValue ? await _spaceService.GetByTypeAsync(type.Value) : await _spaceService.GetAllAsync();
        ViewBag.Types = Enum.GetValues(typeof(SpaceType)).Cast<SpaceType>()
            .Select(t => new SelectListItem { Value = ((int)t).ToString(), Text = t.ToString() });
        ViewBag.SelectedType = type;
        return View(spaces);
    }

    public async Task<IActionResult> Details(int id)
    {
        var space = await _spaceService.GetByIdAsync(id);
        if (space == null) return NotFound();
        return View(space);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Space space)
    {
        if (!ModelState.IsValid) return View(space);
        var response = await _spaceService.CreateAsync(space);
        if (response.Success == false) { ModelState.AddModelError("", response.Message ?? "Unknown error"); return View(space); }
        TempData["Success"] = "Space registered successfully.";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var space = await _spaceService.GetByIdAsync(id);
        if (space == null) return NotFound();
        return View(space);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Space space)
    {
        if (!ModelState.IsValid) return View(space);
        var response = await _spaceService.UpdateAsync(space);
        if (response.Success == false) { ModelState.AddModelError("", response.Message ?? "Unknown error"); return View(space); }
        TempData["Success"] = "Space updated successfully.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Destroy(int id)
    {
        var response = await _spaceService.DeleteAsync(id);
        TempData[response.Success == true ? "Success" : "Error"] =
            response.Success == true ? "Space deleted successfully." : (response.Message ?? "Unknown error");
        return RedirectToAction("Index");
    }
}