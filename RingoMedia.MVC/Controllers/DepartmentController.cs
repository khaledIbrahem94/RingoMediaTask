using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RingoMedia.MVC.Data;
using RingoMedia.MVC.Interface;
using RingoMedia.MVC.Models.DataBase;
using RingoMedia.MVC.Models.ViewModel;

namespace RingoMedia.MVC.Controllers
{
    public class DepartmentController(AppDbContext _context, IFileUploadService _upload, IWebHostEnvironment _hostEnvironment) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var viewModel = new ListDepartmentViewModel
            {
                Departments = new List<DepratmentWithChilds>(),
            };

            List<Department> departments = await _context.Departments
           .Include(d => d.SubDepartments)
           .ToListAsync();

            foreach (var department in departments.Where(x => x.ParentDepartmentId == null))
            {
                viewModel.Departments.Add(new DepratmentWithChilds
                {
                    Department = department,
                    Childs = departments
                    .Where(d => d.ParentDepartmentId == department.Id).ToList()
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _context.Departments.ToListAsync();
            ViewBag.Action = "Create";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var department = new Department
                {
                    Name = model.Name,
                    ParentDepartmentId = model.ParentDepartmentId
                };

                if (model.LogoFile != null)
                {
                    string uniqueFileName = _upload.UploadFile(model.LogoFile,null);
                    department.Logo = uniqueFileName;
                }

                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Action = "Edit";

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var model = new CreateDepartmentViewModel
            {
                Id = department.Id,
                Name = department.Name,
                ParentDepartmentId = department.ParentDepartmentId,
                LogoPath = department.Logo
            };

            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateDepartmentViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                {
                    return NotFound();
                }

                department.Name = model.Name;
                department.ParentDepartmentId = model.ParentDepartmentId;

                if (model.LogoFile != null)
                {
                    string uniqueFileName = _upload.UploadFile(model.LogoFile, department.Logo);
                    department.Logo = uniqueFileName;
                }

                _context.Update(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View("Create", model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            _upload.DeleteImage(department.Logo);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveParent(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            department.ParentDepartmentId = null;
            _context.Update(department);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { id });
        }
    }
}
