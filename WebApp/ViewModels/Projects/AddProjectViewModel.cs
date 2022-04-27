using Microsoft.AspNetCore.Mvc.Rendering;

namespace TestProject.ViewModels.Projects;

public class AddProjectViewModel
{
    public AddProjectDto AddProjectDto { get; set; }
    
    public List<SelectListItem> EmployeesListItems { get; set; }
}