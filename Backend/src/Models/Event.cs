using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Event
{
    public Event(string name, DateTime date, int attenddee)
    {
        this.Name = name;
        // this.Date = date;
        this.Attendee = attenddee;
    }

    [Key]
    public int EventID { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public int Attendee { get; set; }


}
