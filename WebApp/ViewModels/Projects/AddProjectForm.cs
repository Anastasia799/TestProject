using System.ComponentModel.DataAnnotations;

namespace TestProject.ViewModels.Projects;

public class AddProjectForm
{
    [Required] [MaxLength(50)] public string Name { get; set; }


    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Priority { get; set; }

    [Required] [MaxLength(50)] public string ExecutiveCompanyName { get; set; }
    [Required] [MaxLength(50)] public string CustomerCompanyName { get; set; }

    public int ProjectManagerId { get; set; }
    public List<int> EmployeesIds { get; set; }
}