using Domain.Entities;

namespace TestProject.ViewModels;

public class FilterViewModel
{
    public IEnumerable<Project> Projects { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public int Priority { get; set; }
}