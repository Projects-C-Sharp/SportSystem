using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportSistem.Models.Entities;
using SportSistem.Services.Interfaces;

namespace SportSistem.Controllers;

public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IUserService _userService;
    private readonly ISpaceService _spaceService;

    public ReservationController(IReservationService reservationService, IUserService userService, ISpaceService spaceService)
    {
        _reservationService = reservationService;
        _userService = userService;
        _spaceService = spaceService;
    }

    public async Task<IActionResult> Index()
        => View(await _reservationService.GetAllAsync());

    public async Task<IActionResult> Details(int id)
    {
        var reservation = await _reservationService.GetByIdAsync(id);
        if (reservation == null) return NotFound();
        return View(reservation);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Users = new SelectList(await _userService.GetAllAsync(), "Id", "Name");
        ViewBag.Spaces = new SelectList(await _spaceService.GetAllAsync(), "Id", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Reservation reservation)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Users = new SelectList(await _userService.GetAllAsync(), "Id", "Name");
            ViewBag.Spaces = new SelectList(await _spaceService.GetAllAsync(), "Id", "Name");
            return View(reservation);
        }
        var response = await _reservationService.CreateAsync(reservation);
        if (response.Success == false) { ModelState.AddModelError("", response.Message ?? "Unknown error"); 
            ViewBag.Users = new SelectList(await _userService.GetAllAsync(), "Id", "Name");
            ViewBag.Spaces = new SelectList(await _spaceService.GetAllAsync(), "Id", "Name");
            return View(reservation); }
        TempData["Success"] = "Reservation created successfully.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var response = await _reservationService.CancelAsync(id);
        TempData[response.Success == true ? "Success" : "Error"] =
            response.Success == true ? "Reservation cancelled successfully." : (response.Message ?? "Unknown error");
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ByUser(int userId)
    {
        var reservations = await _reservationService.GetByUserAsync(userId);
        return View("Index", reservations);
    }

    public async Task<IActionResult> BySpace(int spaceId)
    {
        var reservations = await _reservationService.GetBySpaceAsync(spaceId);
        return View("Index", reservations);
    }
}