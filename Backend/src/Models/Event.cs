using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Event
{
    public Event(string title, string description, DateTime? date)
    {
        this.Title = title;
        this.Description = description;
        this.Date = date;
    }

    [Key]
    [Column(Order = 0)]
    public int ID { get; set; }
    [Column(Order = 1)]
    public string Title { get; set; }
    [Column(Order = 2)]
    public string Description { get; set; }
    [Column(Order = 3)]
    public DateTime? Date { get; set; }

    public ICollection<EventAttendance> EventAttendances { get; set; } = new List<EventAttendance>();
}
