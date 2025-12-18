using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Event
{

    public Event(EventType type, string title, string description, DateTime? date)
    {
        this.Type = type;
        this.Title = title;
        this.Description = description;
        this.Date = date;
    }

    [Key]
    [Column(Order = 0)]
    public int ID { get; set; }
    [Column(Order = 1)]
    public EventType Type { get; set; }
    [Column(Order = 2)]
    public string Title { get; set; }
    [Column(Order = 3)]
    public string Description { get; set; }
    [Column(Order = 4)]
    public DateTime? Date { get; set; }

    public ICollection<EventAttendance> EventAttendances { get; set; } = new List<EventAttendance>();
}

public enum EventType : int
{
    Event,
    Meeting,
    Birthday,
    Holiday,
    Training,
    Social,
    Incident

}
