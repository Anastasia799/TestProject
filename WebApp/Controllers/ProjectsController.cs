using Application.Dtos.Project;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestProject.ViewModels.Projects;

namespace TestProject.Controllers;

[Route("[controller]/[action]")]
public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(IProjectService projectService,
        IEmployeeService employeeService,
        IMapper mapper,
        ILogger<ProjectsController> logger)
    {
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        try
        {
            var projects = await _projectService.GetAllAsync(cancellationToken);
            var viewModel = new ProjectIndexViewModel
            {
                Projects = projects
            };

            return View(viewModel);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occured while trying to get all projects");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Add(CancellationToken cancellationToken)
    {
        var employees = await _employeeService.GetAllAsync(cancellationToken);
        var viewModel = new AddProjectViewModel
        {
            EmployeesListItems = employees.Select(x =>
                    new SelectListItem(x.FullName(), x.Id.ToString()))
                .ToList(),
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddProjectDto? addProjectDto, CancellationToken cancellationToken)
    {
        try
        {
            if (addProjectDto is null)
                return RedirectToAction(nameof(Index));

            if (!ModelState.IsValid)
            {
                var employees = await _employeeService.GetAllAsync(cancellationToken);
                var viewModel = new AddProjectViewModel
                {
                    EmployeesListItems = employees.Select(x =>
                            new SelectListItem(x.FullName(), x.Id.ToString()))
                        .ToList(),
                };
                return View(viewModel);
            }

            var createProjectDto = _mapper.Map<CreateProjectDto>(addProjectDto);
            var newProjectId = await _projectService.CreateAsync(createProjectDto, cancellationToken);

            return RedirectToAction("Details", new { Id = newProjectId });
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occured while trying to add project");
            return RedirectToAction(nameof(HomeController.Error), "Home");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> Edit(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var project = await _projectService.GetByIdAsync(id, cancellationToken);
            var viewModel = new EditProjectViewModel
            {
                Project = project,
            };
            return View(viewModel);
        }
        catch (EmployeeNotFoundException)
        {
            _logger.LogWarning("Tried to get not-exiting employee {EmployeeId}", id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occured while trying to get a page to edit employee {EmployeeId}",
                id);
            return RedirectToAction(nameof(HomeController.Error), "Home");
        }
    }


    [HttpPost("{id:int}")]
    public async Task<ActionResult> Edit(int id, EditProjectDto editEmployeeDto, CancellationToken cancellationToken)
    {
        try
        {
            var updateProjectDto = _mapper.Map<UpdateProjectDto>(editEmployeeDto);
            await _projectService.UpdateAsync(id, updateProjectDto, cancellationToken);

            return RedirectToAction(nameof(Details), new { id });
        }
        catch (ProjectNotFoundException)
        {
            _logger.LogWarning("Tried to get not-existing project {ProjectId}", id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred while trying to update project {ProjectId}", id);
            return RedirectToAction(nameof(HomeController.Error), "Home");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        try
        {
            var project = await _projectService.GetByIdAsync(id, cancellationToken);
            var viewModel = new ProjectDetailsViewModel()
            {
                Project = project,
            };
            return View(viewModel);
        }
        catch (ProjectNotFoundException)
        {
            _logger.LogWarning("Tried to get not-existing project {ProjectId}", id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occured while trying get the project {ProjectId} details", id);
            return RedirectToAction(nameof(HomeController.Error), "Home");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _projectService.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("User {UserId} has been deleted", id);
            return Ok();
        }
        catch (ProjectNotFoundException)
        {
            return NotFound();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occured while trying to delete a project");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}