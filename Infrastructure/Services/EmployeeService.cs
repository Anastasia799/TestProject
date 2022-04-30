using Application.Dtos.Employee;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _dbContext;

    public EmployeeService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _dbContext.Employees.ToListAsync(cancellationToken);
        return list;
    }

    public async Task<Employee> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var employee = await _dbContext.Employees
            .AsNoTracking()
            .Include(x => x.Projects)
            .ThenInclude(x => x.Project)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (employee is null)
            throw new EmployeeNotFoundException();
        return employee;
    }

    public async Task UpdateAsync(int id, UpdateEmployeeDto updateEmployeeDto,
        CancellationToken cancellationToken = default)
    {
        var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (employee is null)
            throw new EmployeeNotFoundException();
        employee.Email = updateEmployeeDto.Email;
        employee.FirstName = updateEmployeeDto.FirstName;
        employee.LastName = updateEmployeeDto.LastName;
        employee.MiddleName = updateEmployeeDto.MiddleName;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (employee is null)
            throw new EmployeeNotFoundException();

        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CreateAsync(CreateEmployeeDto createEmployeeDto,
        CancellationToken cancellationToken = default)
    {
        var employee = new Employee
        {
            FirstName = createEmployeeDto.FirstName,
            LastName = createEmployeeDto.LastName,
            MiddleName = createEmployeeDto.MiddleName,
            Email = createEmployeeDto.Email,
        };
        _dbContext.Employees.Add(employee);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return employee.Id;
    }
}