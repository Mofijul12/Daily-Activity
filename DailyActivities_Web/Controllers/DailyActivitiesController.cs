using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using DailyActivities_Web.Models;
using ClosedXML.Excel;

[Authorize]
public class DailyActivitiesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public DailyActivitiesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: DailyActivities
    public async Task<IActionResult> Index()
    {

        var userId = _userManager.GetUserId(User);
        var userActivities = await _context.DailyActivities
            .Where(d => d.UserId == userId)
            .ToListAsync();

        return View(userActivities);
    }


    // GET: DailyActivities/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DailyActivities/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DailyActivity dailyActivity)
    {
        if (ModelState.IsValid)
        {
            dailyActivity.UserId = _userManager.GetUserId(User);
            _context.Add(dailyActivity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(dailyActivity);
    }

    // GET: DailyActivities/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var activity = await _context.DailyActivities.FindAsync(id);
        if (activity == null || (!User.IsInRole("Admin") && activity.UserId != _userManager.GetUserId(User)))
            return NotFound();

        return View(activity);
    }

    // POST: DailyActivities/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DailyActivity dailyActivity)
    {
        if (id != dailyActivity.Id) return NotFound();

        var existingActivity = await _context.DailyActivities.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        if (existingActivity == null || (!User.IsInRole("Admin") && existingActivity.UserId != _userManager.GetUserId(User)))
            return NotFound();

        dailyActivity.UserId = existingActivity.UserId; // Keep original user
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(dailyActivity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DailyActivityExists(dailyActivity.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(dailyActivity);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var activity = await _context.DailyActivities
            .Include(d => d.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (activity == null || (!User.IsInRole("Admin") && activity.UserId != _userManager.GetUserId(User)))
            return NotFound();
        _context.DailyActivities.Remove(activity);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Clear()
    {
        var userId = _userManager.GetUserId(User);

        var userActivities = await _context.DailyActivities
            .Where(d => d.UserId == userId)
            .ToListAsync();

        _context.DailyActivities.RemoveRange(userActivities);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    private bool DailyActivityExists(int id)
    {
        return _context.DailyActivities.Any(e => e.Id == id);
    }

    public async Task<IActionResult> ExportToExcel()
    {
        var userId = _userManager.GetUserId(User);
        var activities = await _context.DailyActivities
            .Where(d => d.UserId == userId)
            .ToListAsync();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Daily Activities");

        // Header
        worksheet.Cell(1, 1).Value = "Date";
        worksheet.Cell(1, 2).Value = "Get Up Time";
        worksheet.Cell(1, 3).Value = "Sleep Time";
        worksheet.Cell(1, 4).Value = "Expense";
        worksheet.Cell(1, 5).Value = "Bad Work";
        worksheet.Cell(1, 6).Value = "Salat";
        worksheet.Cell(1, 7).Value = "Quran";

        // Data
        for (int i = 0; i < activities.Count; i++)
        {
            var activity = activities[i];
            worksheet.Cell(i + 2, 1).Value = activity.Date.ToShortDateString();
            worksheet.Cell(i + 2, 2).Value = activity.GetUpTime?.ToString(@"hh\:mm") ?? "N/A";
            worksheet.Cell(i + 2, 3).Value = activity.SleepTime?.ToString(@"hh\:mm") ?? "N/A";
            worksheet.Cell(i + 2, 4).Value = activity.Expense;
            worksheet.Cell(i + 2, 5).Value = activity.BadWork ? "Yes" : "No";
            worksheet.Cell(i + 2, 6).Value = activity.Salat;
            worksheet.Cell(i + 2, 7).Value = activity.Quran ? "Yes" : "No";
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "DailyActivities.xlsx");
    }
}
