using Application.Dtos.Project;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Database;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Project>> GetAsync(ProjectFilterDto? projectFilterDto,
        SortProjectEnum? sortOrder,
        CancellationToken cancellationToken = default)
    {
        var filter = CreateFilter(projectFilterDto);
        var query = _dbContext.Projects
            .AsNoTracking()
            .Include(x => x.ProjectManager)
            .Where(filter);

        if (sortOrder.HasValue)
        {
            query = sortOrder switch
            {
                SortProjectEnum.NameDesc => query.OrderByDescending(x => x.Name),
                SortProjectEnum.NameAsc => query.OrderBy(x => x.Name),
                SortProjectEnum.PriorityDesc => query.OrderByDescending(x => x.Priority),
                SortProjectEnum.PriorityAsc => query.OrderBy(x => x.Priority),
                _ => query,
            };    
        }

        var result = await query.ToListAsync(cancellationToken);

        return result;
    }

    public async Task<Project> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var project = await _dbContext.Projects
            .AsNoTracking()
            .Include(x => x.Employees)
            .ThenInclude(x => x.Employee)
            .Include(x => x.ProjectManager)
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

    private static ExpressionStarter<Project> CreateFilter(ProjectFilterDto? projectFilterDto)
    {
        var filter = PredicateBuilder.New<Project>(true);
        if (projectFilterDto is null)
            return filter;
        if (projectFilterDto.ShouldFilterByName())
        {
            filter = filter.And(x => x.Name.Contains(projectFilterDto.Name));
        }

        if (projectFilterDto.StartDate.HasValue)
        {
            filter = filter.And(x => x.StartDate >= projectFilterDto.StartDate);
        }

        if (projectFilterDto.EndDate.HasValue)
        {
            filter = filter.And(x => x.EndDate <= projectFilterDto.EndDate);
        }

        if (projectFilterDto.Priority.HasValue)
        {
            filter = filter.And(x => x.Priority == projectFilterDto.Priority);
        }

        return filter;
    }
}