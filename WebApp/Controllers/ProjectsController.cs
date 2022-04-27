using Domain.Entities;
using Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TestProject.Controllers;

public class ProjectsController : Controller
{
    private ApplicationDbContext _dbContext;

    public ProjectsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var projects = await _dbContext.Projects.Include(x => x.Employees).ToListAsync();
        return View(projects);
    }

    public IActionResult Add(CancellationToken cancellationToken)
    {
        var employees = new SelectList(_dbContext.Employees, "Id", "Name");
        ViewBag.Employees = employees;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Project? project, CancellationToken cancellationToken)
    {
        if (project != null)
        {
            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    public ActionResult Edit(int id, CancellationToken cancellationToken)
    {
        var project = _dbContext.Projects.Find(id);
        return View(project);
    }


    [HttpPost]
    public async Task<ActionResult> Edit(Project project, CancellationToken cancellationToken)
    {
        var existingProject = await _dbContext.Projects.FirstOrDefaultAsync(x => x.Id == project.Id);
        if (existingProject != null)
        {
            existingProject.Name = project.Name;
            existingProject.CustomerCompanyName = project.CustomerCompanyName;
            existingProject.ExecutiveCompanyName = project.ExecutiveCompanyName;
            existingProject.StartDate = project.StartDate;
            existingProject.EndDate = project.EndDate;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(project);
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var detailsProject = await _dbContext.Projects.Include(x => x.Employees).FirstOrDefaultAsync(p => p.Id == id);
        return View(detailsProject);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var project = await _dbContext.Projects.FirstOrDefaultAsync(x => x.Id == id);
        if (project != null) _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}