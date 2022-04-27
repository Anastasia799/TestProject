using Application.Dtos.Project;
using Domain.Entities;

namespace Application.Interfaces;

public interface IProjectService
{
    Task<List<Project>> GetAsync(CancellationToken cancellationToken = default);
    Task<Project> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Project> UpdateAsync(UpdateProjectDto project, CancellationToken cancellationToken = default);
    Task<Project> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Project> CreateAsync(CreateProjectDto project, CancellationToken cancellationToken = default);

}