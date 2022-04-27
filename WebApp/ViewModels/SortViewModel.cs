namespace TestProject.ViewModels;

public class SortViewModel
{
    public Sort NameSort { get;  set; }
    public Sort PrioritySort { get; set; }

    public SortViewModel(Sort sortOrder)
    {
        NameSort = sortOrder == Sort.PriorityAsc ? Sort.PriorityDesc : Sort.PriorityAsc;
        PrioritySort = sortOrder == Sort.NameProjectAsc ? Sort.NameProjectDesc : Sort.NameProjectAsc;
    }

    public enum Sort
    {
        NameProjectAsc,
        NameProjectDesc,
        PriorityAsc,
        PriorityDesc
    }
}