namespace TestProject.ViewModels.Projects;

public class EditProjectForm
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Priority { get; set; }
    public string ExecutiveCompanyName { get; set; }
    public string CustomerCompanyName { get; set; }

    public int ProjectManagerId { get; set; }
}