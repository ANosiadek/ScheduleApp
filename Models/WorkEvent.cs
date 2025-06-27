﻿public class WorkEvent
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
    public string UserId { get; set; } = string.Empty;
}
