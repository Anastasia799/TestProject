using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TestProject.ViewModels.Projects;

public class EditProjectViewModel
{
    public Project Project { get; set; }
    public List<SelectListItem> EmployeesListItems { get; set; }
  
}