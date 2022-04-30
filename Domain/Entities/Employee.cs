namespace Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string Email { get; set; }
    
    public List<ProjectEmployee> Projects { get; set; }

    public string FullName() => $"{FirstName} {LastName}";
}