using Kt12Profiles.Data;
using Kt12Profiles.Models;
using Kt12Profiles.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Kt12Profiles.Controllers;

[Route("Users")]
public class UsersController(AppDbContext db) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        return View(await db.Users.AsNoTracking().Include(user => user.Profile).OrderBy(user => user.UserId).ToListAsync());
    }

    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var user = await db.Users.AsNoTracking().Include(user => user.Profile).FirstOrDefaultAsync(user => user.UserId == id);
        return user is null ? NotFound() : View(user);
    }

    [HttpGet("Create")]
    public IActionResult Create() => View(new UserForm());

    [HttpPost("Create")]
    public async Task<IActionResult> Create(UserForm model)
    {
        await CheckEmailAsync(model.Email);
        if (!ModelState.IsValid) return View(model);
        var user = new User
        {
            Name = model.Name.Trim(), Email = model.Email.Trim().ToLowerInvariant(),
            Profile = new UserProfile { Bio = model.Bio.Trim(), City = model.City?.Trim() ?? "" }
        };
        db.Users.Add(user);
        if (!await SaveAsync()) return View(model);
        TempData["Success"] = "Пользователь и профиль добавлены";
        return RedirectToAction(nameof(Details), new { id = user.UserId });
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await db.Users.AsNoTracking().Include(user => user.Profile).FirstOrDefaultAsync(user => user.UserId == id);
        if (user is null) return NotFound();
        return View(new UserForm
        {
            UserId = id, Name = user.Name, Email = user.Email,
            Bio = user.Profile.Bio, City = user.Profile.City
        });
    }

    [HttpPost("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, UserForm model)
    {
        var user = await db.Users.Include(user => user.Profile).FirstOrDefaultAsync(user => user.UserId == id);
        if (user is null) return NotFound();
        model.UserId = id;
        await CheckEmailAsync(model.Email, id);
        if (!ModelState.IsValid) return View(model);
        user.Name = model.Name.Trim();
        user.Email = model.Email.Trim().ToLowerInvariant();
        user.Profile.Bio = model.Bio.Trim();
        user.Profile.City = model.City?.Trim() ?? "";
        if (!await SaveAsync()) return View(model);
        TempData["Success"] = "Данные пользователя и профиля обновлены";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await db.Users.AsNoTracking().Include(user => user.Profile).FirstOrDefaultAsync(user => user.UserId == id);
        return user is null ? NotFound() : View(user);
    }

    [HttpPost("Delete/{id:int}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await db.Users.FindAsync(id);
        if (user is null) return NotFound();
        db.Users.Remove(user);
        await db.SaveChangesAsync();
        TempData["Success"] = "Пользователь и его профиль удалены";
        return RedirectToAction(nameof(Index));
    }

    private async Task CheckEmailAsync(string? email, int? excludedId = null)
    {
        if (string.IsNullOrWhiteSpace(email)) return;
        var normalized = email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(user => user.UserId != excludedId && user.Email == normalized))
        {
            ModelState.AddModelError(nameof(UserForm.Email), "Эта электронная почта уже используется");
        }
    }

    private async Task<bool> SaveAsync()
    {
        try
        {
            await db.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqliteException { SqliteErrorCode: 19 })
        {
            ModelState.AddModelError("", "Не удалось сохранить запись, проверьте уникальность электронной почты");
            return false;
        }
    }
}
