using Domain.Entities;

namespace TestProject.ViewModels.Projects;

public class ProjectDetailsViewModel
{
    public Project Project { get; set; }
    public bool EmployeeHasProjects => Project.Employees.Count != 0;
}