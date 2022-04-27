using Application.Dtos.Employee;
using Domain.Entities;

namespace Application.Interfaces;

public interface IEmployeeService
{
    Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Employee> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UpdateEmployeeDto employee, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreateEmployeeDto employee, CancellationToken cancellationToken = default);
}