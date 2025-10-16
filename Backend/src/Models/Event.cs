using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Event
{
    public Event(string title, DateTime date, string description)
    {
        this.Title = title;
        this.Description = description;
        this.Date = date;
    }

    [Key]
    public int ID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }

    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
