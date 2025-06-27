using System.Collections.Generic;

public class AdminViewModel
{
    public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    public List<WorkEvent> Events { get; set; } = new List<WorkEvent>();
}
