using Application.Dtos.Project;
using Domain.Entities;

namespace TestProject.ViewModels.Projects;

public class ProjectIndexViewModel
{
    public List<Project> Projects { get; set; }
    public ProjectFilterDto Filter { get; set; }
 
}