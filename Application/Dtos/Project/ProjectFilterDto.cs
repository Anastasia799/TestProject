namespace Application.Dtos.Project;

public class ProjectFilterDto
{
    public string Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? Priority { get; set; }

    public bool ShouldFilterByName() => !string.IsNullOrWhiteSpace(Name);
}