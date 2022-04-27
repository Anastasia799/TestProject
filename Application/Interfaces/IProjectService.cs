using Application.Dtos.Project;
using Domain.Entities;

namespace Application.Interfaces;

public interface IProjectService
{
    Task<List<Project>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Project> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UpdateProjectDto project, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreateProjectDto createProjectDto, CancellationToken cancellationToken = default);

}