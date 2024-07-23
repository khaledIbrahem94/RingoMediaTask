using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RingoMedia.MVC.Data;
using RingoMedia.MVC.Interface;
using RingoMedia.MVC.Models.DataBase;

namespace RingoMedia.MVC.Controllers
{
    public class RemindersController(AppDbContext _context, IEmailSender _email) : Controller
    {

        // GET: Reminders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reminders.ToListAsync());
        }

        // GET: Reminders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reminders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reminder reminder)
        {
            if (ModelState.IsValid)
            {
                if (reminder.SendingDateTime < DateTime.Now)
                {
                    return RedirectToAction("Error", "Home", new { message = "Cannot Create a reminder in the past." });
                }


                reminder.HangfireJobId = BackgroundJob.Schedule(() => _email.SendEmailAsync(reminder,null), reminder.SendingDateTime);
                _context.Add(reminder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

          

            return View(reminder);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reminder = await _context.Reminders.FindAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }
            return View(reminder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reminder reminder)
        {
            if (id != reminder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingReminder = await _context.Reminders.FindAsync(id);
                if (existingReminder == null)
                {
                    return NotFound();
                }

                if (reminder.SendingDateTime < DateTime.Now)
                {
                    return RedirectToAction("Error", "Home", new { message = "Cannot Edit a reminder that has already been sent." });
                }

                BackgroundJob.Delete(existingReminder.HangfireJobId);

                existingReminder.Title = reminder.Title;
                existingReminder.SendingDateTime = reminder.SendingDateTime;
                existingReminder.Email = reminder.Email;

                // Schedule a new Hangfire job
                existingReminder.HangfireJobId = BackgroundJob.Schedule(() => _email.SendEmailAsync(reminder,null), reminder.SendingDateTime);

                _context.Update(existingReminder);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(reminder);
        }


        // GET: Reminders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reminder = await _context.Reminders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reminder == null)
            {
                return NotFound();
            }

            if (reminder.SendingDateTime < DateTime.Now)
            {
                return RedirectToAction("Error","Home",new {message= "Cannot delete a reminder that has already been sent." });
            }

            return View(reminder);
        }

        // POST: Reminders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reminder = await _context.Reminders.FindAsync(id);

            if (reminder.SendingDateTime < DateTime.UtcNow)
            {
                ModelState.AddModelError("", "Cannot delete a reminder that has already been sent.");
                return View("Error");
            }

            BackgroundJob.Delete(reminder.HangfireJobId);

            _context.Reminders.Remove(reminder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
