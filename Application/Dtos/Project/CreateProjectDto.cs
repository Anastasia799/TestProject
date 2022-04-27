namespace Application.Dtos.Project;

public class CreateProjectDto
{
    public string Name { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Priority { get; set; }

    public string ExecutiveCompanyName { get; set; }
    public string CustomerCompanyName { get; set; }
    
    public int ProjectManagerId { get; set; }
    
    public List<int> EmployeesIds { get; set; }
}