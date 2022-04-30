using Application.Dtos.Employee;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestProject.ViewModels.Employees;

namespace TestProject.Controllers;

[Route("[controller]/[action]")]
public class EmployeesController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(IEmployeeService employeeService, IMapper mapper, ILogger<EmployeesController> logger)
    {
        _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        try
        {
            var employees = await _employeeService.GetAllAsync(cancellationToken);
            var viewModel = new EmployeeIndexViewModel
            {
                Employees = employees
            };

            return View(viewModel);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occured while trying to get all employees");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    public IActionResult Add(CancellationToken cancellationToken)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddEmployeeDto? addEmployeeDto, CancellationToken cancellationToken)
    {
        try
        {
            if (addEmployeeDto is null)
                return RedirectToAction(nameof(Index));
            if (!ModelState.IsValid)
                return View();

            var createEmployeeDto = _mapper.Map<CreateEmployeeDto>(addEmployeeDto);
            var newEmployeeId = await _employeeService.CreateAsync(createEmployeeDto, cancellationToken);

            return RedirectToAction("Details", new { Id = newEmployeeId });
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occured while trying to add employee");
            return RedirectToAction(nameof(HomeController.Error), "Home");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> Edit(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var employee = await _employeeService.GetByIdAsync(id, cancellationToken);
            var employeeEditViewModel = new EditEmployeeDto
            {
                Email = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
            };
            return View(employeeEditViewModel);
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
    public async Task<ActionResult> Edit(int id, EditEmployeeDto editEmployeeDto, CancellationToken cancellationToken)
    {
        try
        {
            var updateEmployeeDto = _mapper.Map<UpdateEmployeeDto>(editEmployeeDto);
            await _employeeService.UpdateAsync(id, updateEmployeeDto, cancellationToken);

            return RedirectToAction(nameof(Details), new { id });
        }
        catch (EmployeeNotFoundException)
        {
            _logger.LogWarning("Tried to get not-existing employee {employeeId}", id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred while trying to update employee {EmployeeId}", id);
            return RedirectToAction(nameof(HomeController.Error), "Home");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        try
        {
            var employee = await _employeeService.GetByIdAsync(id, cancellationToken);
            var viewModel = new EmployeeDetailsViewModel
            {
                Employee = employee,
            };
            return View(viewModel);
        }
        catch (EmployeeNotFoundException employeeNotFoundException)
        {
            _logger.LogWarning("Tried to get not-existing employee {EmployeeId}", id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occured while trying get the employee {EmployeeId} details", id);
            return RedirectToAction(nameof(HomeController.Error), "Home");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _employeeService.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("User {UserId} has been deleted", id);
            return Ok();
        }
        catch (EmployeeNotFoundException)
        {
            return NotFound();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occured while trying to delete a user");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}