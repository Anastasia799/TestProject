using Application.Dtos.Project;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Project>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _dbContext.Projects.ToListAsync(cancellationToken);
        return list;
    }

    public async Task<Project> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var project = await _dbContext.Projects
            .AsNoTracking()
            .Include(x => x.Employees)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (project is null)
            throw new EmployeeNotFoundException();
        return project;
    }


    public async Task UpdateAsync(int id, UpdateProjectDto updateProjectDto,
        CancellationToken cancellationToken = default)
    {
        var project = await _dbContext.Projects.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (project is null)
            throw new EmployeeNotFoundException();
        project.Name = updateProjectDto.Name;
        project.Priority = updateProjectDto.Priority;
        project.StartDate = updateProjectDto.StartDate;
        project.EndDate = updateProjectDto.EndDate;
        project.ProjectManagerId = updateProjectDto.ProjectManagerId;
        project.CustomerCompanyName = updateProjectDto.CustomerCompanyName;
        project.ExecutiveCompanyName = updateProjectDto.ExecutiveCompanyName;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (employee is null)
            throw new ProjectNotFoundException();

        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CreateAsync(CreateProjectDto createProjectDto,
        CancellationToken cancellationToken = default)
    {
        var project = new Project
        {
            Name = createProjectDto.Name,
            Priority = createProjectDto.Priority,
            StartDate = createProjectDto.StartDate,
            EndDate = createProjectDto.EndDate,
            CustomerCompanyName = createProjectDto.CustomerCompanyName,
            ExecutiveCompanyName = createProjectDto.ExecutiveCompanyName,
            ProjectManagerId = createProjectDto.ProjectManagerId,
            Employees = createProjectDto.EmployeesIds.Select(x => new ProjectEmployee { EmployeeId = x }).ToList()
        };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return project.Id;
    }
}