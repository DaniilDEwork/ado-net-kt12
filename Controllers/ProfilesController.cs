using Kt12Profiles.Data;
using Kt12Profiles.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kt12Profiles.Controllers;

[Route("Profiles")]
public class ProfilesController(AppDbContext db) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        return View(await db.UserProfiles.AsNoTracking().Include(profile => profile.User).OrderBy(profile => profile.UserId).ToListAsync());
    }

    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var profile = await db.UserProfiles.AsNoTracking().Include(profile => profile.User).FirstOrDefaultAsync(profile => profile.UserId == id);
        return profile is null ? NotFound() : View(profile);
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var profile = await db.UserProfiles.AsNoTracking().FirstOrDefaultAsync(profile => profile.UserId == id);
        if (profile is null) return NotFound();
        return View(new ProfileForm { UserId = id, Bio = profile.Bio, City = profile.City });
    }

    [HttpPost("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, ProfileForm model)
    {
        var profile = await db.UserProfiles.FindAsync(id);
        if (profile is null) return NotFound();
        model.UserId = id;
        if (!ModelState.IsValid) return View(model);
        profile.Bio = model.Bio.Trim();
        profile.City = model.City?.Trim() ?? "";
        await db.SaveChangesAsync();
        TempData["Success"] = "Профиль обновлён";
        return RedirectToAction(nameof(Details), new { id });
    }
}
