using Domain.Entities;

namespace TestProject.ViewModels.Employees;

public class EmployeeDetailsViewModel
{
    public Employee Employee { get; set; }

    public bool EmployeeHasProjects => Employee.Projects.Count != 0;
}