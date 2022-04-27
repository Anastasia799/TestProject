using Domain.Enums;

namespace Domain.Entities;

public class Project
{
    public int Id { get; }

    public string Name { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Priority { get; set; }

    public string ExecutiveCompanyName { get; set; }
    public string CustomerCompanyName { get; set; }
    
    public Employee ProjectManager { get; set; }
    public int ProjectManagerId { get; set; }
    
    public List<ProjectEmployee> Employees { get; set; }
}