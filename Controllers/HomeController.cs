using Kt12Profiles.Data;
using Kt12Profiles.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kt12Profiles.Controllers;

public class HomeController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(new HomeModel
        {
            UsersCount = await db.Users.CountAsync(),
            ProfilesCount = await db.UserProfiles.CountAsync()
        });
    }

    public IActionResult Error()
    {
        Response.StatusCode = StatusCodes.Status500InternalServerError;
        return View();
    }
}
